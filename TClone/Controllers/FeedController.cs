using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TClone.Models;
using TClone.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TClone.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly TcDbcontext _login;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFeed _feed;

        public FeedController(TcDbcontext login, IMapper mapper, IWebHostEnvironment environment,IFeed feed)
        {
            _login = login;
            _mapper = mapper;
            _environment = environment;
            _feed = feed;
        }

         // Add Posts 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Posts postReq)
        {
            var post = await _feed.Post(postReq);
            return Ok();

        }
        // Get all post for home
        [HttpGet("feed/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var get = await _feed.Get(id);
            return Ok(get);

        }

        // Get Post For Following Only 
        [HttpGet("followingPost/{id}")]
        public async Task<IActionResult> followingPost(string id)
        {
            var get = await _feed.followingPost(id);
            return Ok(get);
        }


        //Get info 
        [HttpGet ("profile/{id}") ]
        public async Task<IActionResult> GetProfile ([FromRoute] string id )
        {
            var info = await _feed.Profile(id);
            return Ok(info);
        }
        
        //Add Follower
        [HttpPost("follow")]
        public async Task<IActionResult> Follow ([FromBody] Follow follow)
        {
            var folow = await _feed.Follow(follow);
            return Ok();
        }
        
        //Get Follower List
        [HttpGet("follower/{id}")]
        public async Task<IActionResult> Follower ([FromRoute] string id)
        {
            var follower = await _feed.Follower(id);
            return Ok(follower);
        }
        //Get Following List
        [HttpGet("following/{id}")]
        public async Task<IActionResult> Following([FromRoute] string id)
        {
            var following = await _feed.Following(id);
            return Ok(following);
        }
        //UnFollow
        [HttpDelete("unfollow/{id}")]
        public async Task<IActionResult> Unfollow([FromRoute] int id)
        {
            var unfollow = await _feed.Unfollow(id);
            return Ok(unfollow);
        }
        //Get Post for profile 
        [HttpGet("ProfilePost")]
        public async Task<IActionResult> ProfilePost([FromQuery] string poster, [FromQuery] string viewer)
        {
            var posts = await _feed.ProfilePosts(poster, viewer);
            return Ok(posts);
        }


    }
}
