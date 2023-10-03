using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TClone.Data;
using TClone.Models;
using TClone.Services;

namespace TClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly TcDbcontext _pro;
        private readonly IMapper _mapper;
        private readonly IProfile _pofile;

        public ProfileController(TcDbcontext pro, IMapper mapper, IProfile pofile)
        {
            _pro = pro;
            _mapper = mapper;
            _pofile = pofile;
        }

        //Get user info
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetProfile([FromRoute] string id)
        {
            var profile = await _pofile.GetProfile(id);
            return Ok(profile);

        }
        //Delete User
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProfile([FromRoute] Guid id)
        {
            var delete = await _pofile.DeleteProfile(id);
            return Ok(delete);
        }
        //Update User
        
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid id,ProfileDto request)
        {
            var update = await _pofile.UpdateProfile(id, request);
            return Ok(update);

        }
    }
}
