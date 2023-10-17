using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TClone.Data;
using TClone.Models;
using TClone.RepoImplementation;

namespace TClone.Services
{
    public class Auth : GenericRepository<User>, IAuth
    {
        private readonly TcDbcontext _login;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public Auth(TcDbcontext login, IMapper mapper, IWebHostEnvironment environment) : base(login)
        {
            _login = login;
            _mapper = mapper;
            _environment = environment;
        }
        public static User user = new User();



        // For User Registration 
        public async Task<string> Register(UserDto request)
        {
            if (await _login.Users.AnyAsync(u => u.Username == request.Username))
            {
                var resultMessage = new { message = "User Already Exist !!" };
                return (JsonConvert.SerializeObject(resultMessage));

            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.Id = Guid.NewGuid();
            _login.Users.Add(user);
            await _login.SaveChangesAsync();
            var msg = new { message = "User Added" };
            return (JsonConvert.SerializeObject(msg));
        }

        //For Login 
        public async Task<object> Login(LoginDto request)
        {
            var user = await _login.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                var msg = new { message = "User Not Found" };
                return (JsonConvert.SerializeObject(msg));
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                var msgg = new { message = "Wrong Password" };
                return (JsonConvert.SerializeObject(msgg));
            }
            var active = user.Active;
            if (active == false)
            {
                var resultMessage = new { message = "User No Longer Available" };
                return (JsonConvert.SerializeObject(resultMessage));
            }

            string token = CreateToken(user);


            // Map User to UserDto before returning the token

            return (new
            {
                Token = token,
                User = new
                {
                    user.Username,
                    user.Id,
                    user.ContactId

                }
            });
        }








        // METHODS FOR TOKEN GENERATIONS 
        private string CreateToken(User user)
        {
            var key = GenerateSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private SymmetricSecurityKey GenerateSecurityKey()
        {
            byte[] keyBytes = new byte[128];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }

            return new SymmetricSecurityKey(keyBytes);
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

