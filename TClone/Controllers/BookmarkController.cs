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

    }
}
