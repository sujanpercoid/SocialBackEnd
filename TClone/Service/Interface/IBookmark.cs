
using Dto;
using Models;


namespace TClone.Services
{
    public interface IBookmark 
    {
        Task<string> Bookmark(Bookmark bookmark);
        Task<List<BookmarkDto>> GetBookmarks(string id);
        Task<string> DeletSBookmark(object BookmarkId);
        Task<List<Bookmark>> GetBookmarkid(string id);
        Task<string> PostLike(Like like);
        Task<string> RemoveLike(string username,int postId);
        Task<List<Like>> GetLike(string id);
        Task<List<Like>> GetUsername(int id);
    }
}
