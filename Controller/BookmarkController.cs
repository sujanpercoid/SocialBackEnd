using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using TClone.Services;
using Dto;
using Models;
using Microsoft.AspNetCore.Hosting;
using Data;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly TcDbcontext _table;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IBookmark _book;

        public BookmarkController(TcDbcontext table, IMapper mapper, IWebHostEnvironment environment, IBookmark book)
        {
            _table = table;
            _mapper = mapper;
            _environment = environment;
            _book = book;
        }
        //For BookMark Post
        [HttpPost("bookmark")]
        public async Task<IActionResult> Bookmark([FromBody]Bookmark  bookmark)
        {
            var book = await _book.Bookmark(bookmark);
            return Ok(book);
        }

        //Get all BookMarked Post
        [HttpGet("getbookmark/{id}")]
        public async Task<ActionResult> GetBookmarks([FromRoute] string id)
        {
            var bookmark = await _book.GetBookmarks(id);
            return Ok(bookmark); 
        }

        //Delete Single BookMark
        [HttpDelete("deletesbookmark/{id}")]
        public async Task<ActionResult> DeleteSBookmark([FromRoute] int id)
        {
            var bookmark = await _book.DeletSBookmark(id);
            return Ok(bookmark);
        }
        //Get Bookmarked Post
        [HttpGet("bookmarkedPost/{id}")]
        public async Task<IActionResult>BookmarkedPost([FromRoute] string id)
        {
            var bookmark = await _book.GetBookmarkid(id);
            return Ok(bookmark);
        }

        //Get Like Data 
        [HttpPost("like")]
        public async Task<IActionResult>Like([FromBody]Like like)
        {
            var likes = await _book.PostLike(like);
            return Ok(like);
        }
        //Remove Likes
        [HttpDelete("removelike")]
        public async Task <IActionResult> RemoveLike([FromQuery] string username , [FromQuery] int postId)
        {
            var rlike = await _book.RemoveLike(username,postId);
            return Ok(rlike);
        }
        //Get All Likes
        [HttpGet("getLikes/{id}")]
        public async Task<IActionResult> GetLikes([FromRoute] string id)
        {
            var llike = await _book.GetLike(id);
            return Ok(llike);
        }
        //Get all like username
        [HttpGet("likeuser/{id}")]
        public async Task<IActionResult> Getuser([FromRoute]int id)
        {
            var user = await _book.GetUsername(id);
            return Ok(user);
        }

    }
}
