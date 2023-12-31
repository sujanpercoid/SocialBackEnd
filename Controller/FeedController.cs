﻿using AutoMapper;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models;

using TClone.Repository;
using TClone.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly TcDbcontext _login;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFeed _feed;
        private readonly IGenericRepository<Posts> _feedrepo;
       

        public FeedController(TcDbcontext login, IMapper mapper, IWebHostEnvironment environment,IFeed feed,IGenericRepository<Posts> feedrepo)
        {
            _login = login;
            _mapper = mapper;
            _environment = environment;
            _feed = feed;
            _feedrepo = feedrepo;
            
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
        //Get Post for profile 
        [HttpGet("PostForLikes")]
        public async Task<IActionResult> PostForLikes([FromQuery] string poster, [FromQuery] string viewer)
        {
            var posts = await _feed.GetPostForLikes(poster, viewer);
            return Ok(posts);
        }

        //Get all my posts
        [HttpGet("GetMyPosts/{id}")]
        public async Task <IActionResult>GetMyPosts([FromRoute] string id)
        {
            var myposts = await _feed.MyPosts(id);
            return Ok(myposts);
        }
        //Get all my likes
        [HttpGet("mylikes/{id}")]
        public async Task<IActionResult>MyLikes([FromRoute] string id)
        {
            var mylikes = await _feed.MyLikedPosts(id);
            return Ok(mylikes);
        }
        //Get post for edit
        [HttpGet("postedit/{id}")]
        public async Task<IActionResult> EditPost([FromRoute] int id)
        {
            var post = await _feed.EditPosts(id);
            return Ok(post);
        }
        //Update Posts
        [HttpPut("updatepost/{id}")]
        public async Task<IActionResult>UpdatePost([FromRoute] int id ,[FromBody]Posts post)
        {
            var update = await _feed.UpdatedPost(id, post);
            return Ok(update);
        }
        //Delete Post 
        [HttpDelete("deletepost/{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            string result = await _feed.Delete(id);
            return Ok(result);
        }
        //get a single post for edit
        [HttpGet("getsinglepost/{id}")]
        public async Task<IActionResult> getsinglepost([FromRoute] int id)
        {
            var post = await _feed.getsinglepost(id);
            return Ok(post);
        }
        //add cmt 
        [HttpPost("cmt")]
        public async Task<IActionResult> addcmt([FromBody] Comment cmt)
        {
            var cmts = await _feed.AddCmt(cmt);
            return Ok(cmts);
        }

        //get all cmt post
        [HttpGet("cmt/{id}")]
        public async Task<IActionResult> getcmt ([FromRoute] int id)
        {
            var cmt = await _feed.GetComment(id);
            return Ok(cmt);
        }
        
        //get username
        [HttpGet("username")]
        public async Task<IActionResult> getusername ()
        {
            var uname =await _feed.GetUserName();
            return Ok(uname);
        }
        

    }
}
