using AppCore.DTOs;
using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IAuthService
    {
        Task<UserForDetailedDTO> Register(UserForRegisterDto userForRegisterDto);
        Task<object> Login(UserForLoginDto userForLoginDto);
    }
}
