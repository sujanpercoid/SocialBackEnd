using AutoMapper;
using Dapper;
using Data;
using Dto;
using GenericRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using Repository;

namespace TClone.Services
{
    public class Feed : GenericRepository<Posts>, IFeed
    {
        private readonly TcDbcontext _feed;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Posts> _feedRepository;
        private readonly IGenericRepository<Follow> _followRepository;
        private readonly IGenericRepository<Comment> _commentRepository;

        public Feed(TcDbcontext feed, IMapper mapper, IWebHostEnvironment environment,IConfiguration config, IGenericRepository<Posts> feedRepository,
           IGenericRepository<Follow> followRepository,IGenericRepository<Comment> commentRepository) : base(feed)
        {
            _feed = feed;
            _mapper = mapper;
            _environment = environment;
            _config = config;
            _feedRepository = feedRepository;
            _followRepository = followRepository;
            _commentRepository = commentRepository;
        }

        //Connection string for sql for whole class
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("conn"));
        }

        //Post using repo pattern
        public async Task<string> Post(Posts req)
        {
            await _feedRepository.Add(req);
            var resultMessage = new { message = "User Unfollowed!!" };
            return (JsonConvert.SerializeObject(resultMessage));
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
        public async Task<string>Unfollow(object FollowId)
        {
            await _followRepository.Delete(FollowId);
            
            var resultMessage = new { message = "User Unfollowed!!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
        //Get Post for profile
        public async Task<List<PostDto>> ProfilePosts(string poster,string viewer)
        {
             
            using var connection = CreateConnection();
            var sqlQuery = @"SELECT DISTINCT p.*, (
                           SELECT COUNT(*) 
                           FROM likes l 
                          WHERE l.PostId = p.PostId
                         ) AS TotalLikes
                          FROM Posts p
                            LEFT JOIN likes l ON l.PostId = p.PostId
                          WHERE (p.username = @poster) AND (p.[View] = 'public' OR (p.[View] = 'follower' AND EXISTS (
                        SELECT 1
                       FROM Follows F
                        WHERE F.Follows = p.Username 
                        AND F.Username = @viewer
                            )))
                    ORDER BY p.PostId DESC;";
            var parameter = new { poster = poster, viewer = viewer };
            
            var info = await connection.QueryAsync<PostDto>(sqlQuery, parameter);
            return info.ToList();

        }

        //Get post for profile likes
        public async Task<List<PostDto>>GetPostForLikes(string poster, string viewer)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select p.*,(
                           SELECT COUNT(*) 
                           FROM likes l 
                          WHERE l.PostId = p.PostId
                         ) AS TotalLikes
                            from likes l
                            left join  posts p on
                           p.postId = l.PostId
                           WHERE (l.username =@poster) AND (p.[View] = 'public' OR (p.[View] = 'follower' AND EXISTS (
                          SELECT 1
                           FROM Follows F
                          WHERE F.Follows = p.Username 
                          AND F.Username = @viewer
                              )))
                          ORDER BY p.PostId DESC;";
            var parameter = new { poster = poster, viewer = viewer };
            var info = await connection.QueryAsync<PostDto>(sqlQuery, parameter);
            return info.ToList();

        }
        //Get all my posts
        public async Task<List<Posts>>MyPosts(string  id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select *
                             from posts 
                           where username =@username
                            ORDER BY PostId DESC ;";
            var parameter = new { username = id};
            var info = await connection.QueryAsync<Posts>(sqlQuery, parameter);
            return info.ToList();
        }
        //Get all my Liked Post
        public async Task<List<Posts>>MyLikedPosts(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"SELECT  p.*
                            FROM  likes l
                             right JOIN Posts p ON l.PostId = p.PostId
                              WHERE l.username = @username
						  ORDER BY PostId DESC;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<Posts>(sqlQuery, parameter);
            return info.ToList();
        }

        //Get Indivisual post for edit 
        public async Task<Posts>EditPosts(int id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select *
                              from posts
                           where PostId =@id ;";
            var parameter = new { id = id };
            var info = await connection.QueryFirstOrDefaultAsync<Posts>(sqlQuery, parameter);
            return info;
        }

        //For updated Post
        public async Task <string> UpdatedPost(int  id,Posts req)
        {
            var post = await _feed.Posts.FirstOrDefaultAsync(u => u.PostId == id);

            if (post == null)
            {
                var msg = new { message = "not found" };
                return (JsonConvert.SerializeObject(msg));

            }
            post.Username = req.Username;
            post.View = req.View;
            post.Post = req.Post;
            

            await _feed.SaveChangesAsync();
            var resultMessage = new { message = "Edited" };
            return (JsonConvert.SerializeObject(resultMessage));
        }

        //Delete Post
        //public async Task<string>DeletePost(int id)
        //{
        //    using var connection = CreateConnection();
        //    var sqlQuery = @"delete from posts
        //                     where postId = @id ";
        //    var parameter = new { id = id };
        //    var info = await connection.QueryFirstOrDefaultAsync<Posts>(sqlQuery, parameter);
        //    var resultMessage = new { message = "Post Deleted " };
        //    return (JsonConvert.SerializeObject(resultMessage));

        //}

        public async Task<string> Delete(object PostId)
        {
            await _feedRepository.Delete(PostId);
            return "deleted";
        }

        //get single post for view
        public async Task<PostDto> getsinglepost(int id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"SELECT Distinct p.*, (SELECT COUNT(*) FROM likes l WHERE l.PostId = p.PostId) AS TotalLikes
                             FROM posts p
                             LEFT JOIN likes l ON l.PostId = p.PostId
                             WHERE p.PostId =@id;";
            var parameter = new { id = id };
            var post = await connection.QueryFirstOrDefaultAsync<PostDto>(sqlQuery, parameter);
            return post;
        }

        //add comment
        public async Task <string> AddCmt (Comment cmt)
        {
            await _commentRepository.Add(cmt);
            var resultMessage = new { message = "Cmt Added !!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }

        //get all cmt for post
        public async Task<List<Comment>> GetComment(int id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select *
                             from comments
                             where postid = @id ; ";
            var parameter = new { id = id };
            var post = await connection.QueryAsync<Comment>(sqlQuery, parameter);
            return post.ToList();
        }

        //get all username
        public async Task<List<User>> GetUserName()
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select name
                             from Users;";
            var name = await connection.QueryAsync<User>(sqlQuery);
            return name.ToList();
        }



    }
}
