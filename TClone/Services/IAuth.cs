using TClone.Models;
using TClone.Repository;

namespace TClone.Services
{
    public interface IAuth : IGenericRepository<User>
    {
        Task<string> Register(UserDto request);
        Task<object> Login(LoginDto request);
    }
}
