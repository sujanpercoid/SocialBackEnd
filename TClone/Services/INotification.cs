using TClone.Models;

namespace TClone.Services
{
    public interface INotification
    {
        Task<List<Notification>> Notification(string id);
        Task<string> DeleteAllNoti(string id);
        Task<string> DeleteNoti(int id);
    }
}
