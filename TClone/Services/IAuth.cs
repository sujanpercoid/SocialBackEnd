using TClone.Models;

namespace TClone.Services
{
    public interface IAuth
    {
        Task<string> Register(UserDto request);
        Task<object> Login(LoginDto request);
    }
}
