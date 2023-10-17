using AutoMapper;
using Dapper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using TClone.Data;
using TClone.Models;
using TClone.RepoImplementation;
using TClone.Repository;

namespace TClone.Services
{
    public class Bookmarkss:  IBookmark
    {
        private readonly TcDbcontext _book;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Bookmark> _feedRepository;

        public Bookmarkss(TcDbcontext book, IMapper mapper, IWebHostEnvironment environment, IConfiguration config, IGenericRepository<Bookmark> feedRepository)
        {
            _book = book;
            _mapper = mapper;
            _environment = environment;
            _config = config;
            _feedRepository = feedRepository;
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
        public async Task<string> DeletSBookmark(object BookmarkId)
        {
            await _feedRepository.Delete(BookmarkId);
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
        //public async Task<string>PostLike(Like like)
        //{
        //    var follow = await _book.Likes.AddAsync(like);
        //    await _book.SaveChangesAsync();
        //    var resultMessage = new { message = " Like Added !!" };
        //    return (JsonConvert.SerializeObject(resultMessage));

        //}  

        //Post Like Info 
        public async Task<string> PostLike(Like like)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                // Step 1: Add a "like" to the database
                var follow = await connection.ExecuteAsync("INSERT INTO Likes (username, PostId) VALUES (@username, @PostId)", like);

                // Step 2: Retrieve the latest like information
                var latestLikeInfo = await connection.QueryFirstOrDefaultAsync<LikerInfo>(
                    "SELECT TOP 1 l.username AS PostLiker, p.username AS Poster " +
                    "FROM Likes l " +
                    "LEFT JOIN Posts p ON p.PostId = l.PostId " +
                    "ORDER BY l.likeid DESC");

                // Step 3: Insert a notification
                await connection.ExecuteAsync(
                    "INSERT INTO Notifications (Username, Messgae) " +
                    "VALUES (@Username, @Messgae)",
                    new { Username = latestLikeInfo.Poster, Messgae = latestLikeInfo.PostLiker + " liked Your Post" });

                // Optionally, you can return a success message as JSON
                var resultMessage = new { message = "Like Added !!" };
                return JsonConvert.SerializeObject(resultMessage);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                // You can return an error message as JSON or throw an exception as needed.
                var errorMessage = new { error = "An error occurred while adding the like." };
                return JsonConvert.SerializeObject(errorMessage);
            }
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
        //Get All Like Username
        public async Task<List<Like>>GetUsername(int id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"
                            select username
                            from Likes
                            where PostId =@id;";
            var parameter = new { id = id };
            var info = await connection.QueryAsync<Like>(sqlQuery, parameter);
            return info.ToList();
        }
        
    }
}
