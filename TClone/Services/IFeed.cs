using TClone.Models;

namespace TClone.Services
{
    public interface IFeed
    {
        Task<string> Post(Posts req);
        Task<List<PostDto>> Get(string id);
        Task<List<PostDto>> followingPost(string id);
        Task<string> Follow (Follow req);
        Task<IEnumerable<ProfileNameDto>> Profile(string id);
        Task<List<FollowerDto>> Follower(string id);
        Task<List<FollowingDto>> Following(string id);
        Task<string> Unfollow(int id);
        Task<List<Posts>> ProfilePosts(string poster, string viewer);


    }
}
