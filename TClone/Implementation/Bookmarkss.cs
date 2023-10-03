using AutoMapper;
using Dapper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using TClone.Data;
using TClone.Models;

namespace TClone.Services
{
    public class Bookmarkss: IBookmark
    {
        private readonly TcDbcontext _book;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public Bookmarkss(TcDbcontext book, IMapper mapper, IWebHostEnvironment environment, IConfiguration config)
        {
            _book = book;
            _mapper = mapper;
            _environment = environment;
            _config = config;
        }
        //Connection string for sql for whole class
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("conn"));
        }
        //Put Bookmark Info
        public async Task<string>Bookmark(Bookmark bookmark)
        {
            var follow = await _book.Bookmarks.AddAsync(bookmark);
            await _book.SaveChangesAsync();
            var resultMessage = new { message = " Bookmark Added !!" };
            return (JsonConvert.SerializeObject(resultMessage));

        }
        // Get All Bookmarks
        public async Task<List<BookmarkDto>> GetBookmarks(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select
                           p.postid,p.post,p.Username,p.[view],b.bookmarkid
                           from bookmarks b
                           join posts p on p.postid =b.PostId
                            where b.Username  =@username;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<BookmarkDto>(sqlQuery, parameter);
            return info.ToList();
        }
        //Delete Single Bookmark
        public async Task<string> DeletSBookmark(int id)
        {
            var dbook = await _book.Bookmarks.FindAsync(id);
             _book.Bookmarks.Remove(dbook);
            await _book.SaveChangesAsync();
            var resultMessage = new { message = "  Bookmark Removed !!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
        //Get bookmark post 
        public async Task<List<Bookmark>> GetBookmarkid(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select postid
                            from Bookmarks
                           where username =@username";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<Bookmark>(sqlQuery, parameter);
            return info.ToList();

        }
        //Post Like Info 
        public async Task<string>PostLike(Like like)
        {
            var follow = await _book.Likes.AddAsync(like);
            await _book.SaveChangesAsync();
            var resultMessage = new { message = " Like Added !!" };
            return (JsonConvert.SerializeObject(resultMessage));

        }
        //Remove Like Info
        public async Task<string>RemoveLike (string username, int postId)
        {
            
            using var connection = CreateConnection();
            var sqlQuery = @"delete from likes
                         where username =@username
                         and PostId = @id;";
            var parameter = new { username = username,id = postId };
            var info = await connection.QueryAsync<Like>(sqlQuery, parameter);
            var resultMessage = new { message = " Like Removed !!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
        //Get All Like Data
        public async Task<List<Like>> GetLike(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select *
                             from likes
                              where username = @username ;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<Like>(sqlQuery,parameter);
            return info.ToList();

        }
    }
}
