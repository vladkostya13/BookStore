using BookStore.API.Dtos.User;
using System.Threading.Tasks;

namespace BookStore.API.Managers.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserAuthenticationDto userAuthenticationDto);
        Task<string> CreateToken();
    }
}