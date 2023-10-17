using TClone.Models;
using TClone.Repository;

namespace TClone.Services
{
    public interface INotification : IGenericRepository<Notification>
    {
        Task<List<Notification>> Notification(string id);
        Task<string> DeleteAllNoti(string id);
        Task<string> DeleteNoti(int id);
    }
}
