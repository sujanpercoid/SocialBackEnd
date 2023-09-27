using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TClone.Data;
using TClone.Models;
using TClone.Services;

namespace TClone
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TcDbcontext _login;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IAuth _iauth;

        public AuthController(TcDbcontext login, IMapper mapper, IWebHostEnvironment environment, IAuth iauth)
        {
            _login = login;
            _mapper = mapper;
            _environment = environment;
            _iauth = iauth;
        }

        //For Registration
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var reg = await _iauth.Register(request);
            var response = new { message = "User Added" };
            return Ok(response);
        }

        //For Login 
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginDto request)
        {
            var log = await _iauth.Login(request);
            return Ok(log);
        }
    }
}
