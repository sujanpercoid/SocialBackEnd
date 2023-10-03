using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TClone.Data;
using TClone.Models;

namespace TClone.Services
{
    public class Feed : IFeed
    {
        private readonly TcDbcontext _feed;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public Feed(TcDbcontext feed, IMapper mapper, IWebHostEnvironment environment,IConfiguration config)
        {
            _feed = feed;
            _mapper = mapper;
            _environment = environment;
            _config = config;
        }

        //Connection string for sql for whole class
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("conn"));
        }
        public async Task<string> Post(Posts req)
        {
            var addp = await _feed.Posts.AddAsync(req);
            await _feed.SaveChangesAsync();
            return "Post Added";
        }
        //get all post for home
        public async Task <List<PostDto>> Get (string id )
        {
            using var connection = CreateConnection();
            var sqlQuery = @"SELECT Distinct p.*, (SELECT COUNT(*) FROM likes l WHERE l.PostId = p.PostId) AS TotalLikes
                             FROM posts p
                             LEFT JOIN likes l ON l.PostId = p.PostId
                             WHERE P.[View] = 'public'
                             OR (P.[View] = 'follower' AND EXISTS (
                             SELECT 1
                             FROM Follows F
                             WHERE F.Username = @username
                             AND F.Follows = P.Username
                               ))
                             order by p.PostId desc; ";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<PostDto>(sqlQuery, parameter);
            return info.ToList();
        }

        //get post for following only
        public async Task<List<PostDto>> followingPost(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"SELECT  Distinct p.*,(SELECT COUNT(*) FROM likes l WHERE l.PostId = p.PostId) AS TotalLikes
                          FROM posts p
                         JOIN follows f ON p.username = f.follows
						 left join likes l on l.PostId = p.PostId
                             WHERE f.username = @username
                           ORDER BY p.postid DESC;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<PostDto>(sqlQuery, parameter);
            return info.ToList();
        }

        public async Task <string> Follow (Follow req)
        {
            string username = req.Username;
            string follows = req.Follows;
            string notificationMsg = $"{username} Followed You !!";
            // Create an instance of the Notification class
            var notification = new Notification
            {
                Username = follows,
                Messgae = notificationMsg
            };
            _feed.Notifications.Add(notification);
            await _feed.SaveChangesAsync();


            var follow = await _feed.Follows.AddAsync(req);
            await _feed.SaveChangesAsync();
            return "Added";
        }
        public async Task <IEnumerable<ProfileNameDto>>Profile(string id)
        {
           using var  connection =CreateConnection();
            var sqlQuery = @"SELECT 
                           (SELECT name from users where username = @username) as name,
	                       (SELECT username from users where username = @username) as username,
                           (SELECT COUNT(username) FROM Follows WHERE username = @username) as follows,
                           (SELECT COUNT(follows) FROM Follows WHERE follows = @username) as followers;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<ProfileNameDto>(sqlQuery, parameter);
            return info;

        }
        public async Task<List<FollowerDto>>Follower(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"
                             select username,followid
                             from follows
                             where follows = @username;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<FollowerDto>(sqlQuery, parameter);
            return info.ToList();

        }
        public async Task<List<FollowingDto>>Following(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"
                             select follows
                             from follows
                             where username = @username;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<FollowingDto>(sqlQuery, parameter);
            return info.ToList();

        }

        // UnFollow
        public async Task<string>Unfollow(int id)
        {
            var unfollow = await _feed.Follows.FindAsync(id);
            _feed.Follows.Remove(unfollow);
            await _feed.SaveChangesAsync();
            var response = new { Message = "Deleted" };
            var resultMessage = new { message = "User Already Exist !!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
        //Get Post for profile
        public async Task<List<Posts>> ProfilePosts(string poster,string viewer)
        {
             
            using var connection = CreateConnection();
            var sqlQuery = @"SELECT P.*
                           FROM Posts P
                           WHERE P.username = @poster AND
                           P.[View] = 'public'
                           OR (P.[View] = 'follower' AND EXISTS (
                          SELECT 1
                          FROM Follows F
                          WHERE F.Username = @viewer
                          AND F.Follows = P.Username
                          ))
                       order by PostId desc;";
            var parameter = new { poster = poster, viewer = viewer };
            
            var info = await connection.QueryAsync<Posts>(sqlQuery, parameter);
            return info.ToList();

        }
    }
}
