using TClone.Models;

namespace TClone.Services
{
    public interface IBookmark
    {
        Task<string> Bookmark(Bookmark bookmark);
        Task<List<BookmarkDto>> GetBookmarks(string id);
        Task<string> DeletSBookmark(int id);
        Task<List<Bookmark>> GetBookmarkid(string id);
    }
}
