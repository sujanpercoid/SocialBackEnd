using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.SqlClient;
using TClone.Data;
using TClone.Models;
using TClone.Services;

namespace TClone.Implementation
{
    public class Profile: IProfile
    {
        private readonly TcDbcontext _pro;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public Profile(TcDbcontext pro,IMapper mapper, IWebHostEnvironment environment, IConfiguration config)
        {
           _pro = pro;
            _mapper = mapper;
            _environment = environment;
            _config = config;
        }
        //Connection string for sql for whole class
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("conn"));
        }

        //To Get Profile Info
        public async Task<ProfileDto>GetProfile(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select contactid,id,name,email,username,phone
                              from users
                              where username = @username;";
            var parameter = new { username = id };
            var info = await connection.QueryFirstOrDefaultAsync<ProfileDto>(sqlQuery, parameter);
            return info;

        }

        //To Delete Profile
        public async Task<string>DeleteProfile (Guid id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"update users
                             set Active = 0
                             where id = @id ;";
            var parameter = new { id = id };
            var info = await connection.QueryFirstOrDefaultAsync<ProfileDto>(sqlQuery, parameter);
            var resultMessage = new { message = "User Deleted !!" };
            return (JsonConvert.SerializeObject(resultMessage));

        }
        //update Profile
        public async Task<string> UpdateProfile(Guid id, ProfileDto request)
        {
            var profile = await _pro.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (profile == null)
            {
                var msg = new { message = "not found" };
                return (JsonConvert.SerializeObject(msg));

            }
            profile.Username = request.Username;
            profile.Email = request.Email;
            profile.Phone = request.Phone;
            profile.Name = request.Name;

            await _pro.SaveChangesAsync();
            var resultMessage = new { message = "Edited" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
    }
}
