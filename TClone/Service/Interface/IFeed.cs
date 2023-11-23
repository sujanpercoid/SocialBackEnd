
using Dto;
using Models;
using Repository;

namespace TClone.Services
{
    public interface IFeed : IGenericRepository<Posts>
    {
        Task<string> Post(Posts req);
        Task<List<PostDto>> Get(string id);
        Task<List<PostDto>> followingPost(string id);
        Task<string> Follow (Follow req);
        Task<IEnumerable<ProfileNameDto>> Profile(string id);
        Task<List<FollowerDto>> Follower(string id);
        Task<List<FollowingDto>> Following(string id);
        Task<string> Unfollow(object FollowId);
        Task<List<PostDto>> ProfilePosts(string poster, string viewer);
        Task<List<PostDto>> GetPostForLikes(string poster, string viewer);
        Task<List<Posts>> MyPosts(string id);
        Task<List<Posts>> MyLikedPosts(string id);
        Task<Posts> EditPosts(int id);
        Task<string> UpdatedPost(int id, Posts req);
        Task<PostDto> getsinglepost(int id);
        Task<string> AddCmt(Comment cmt);
        Task<List<Comment>> GetComment(int id);
        Task<List<User>> GetUserName();




    }
}
