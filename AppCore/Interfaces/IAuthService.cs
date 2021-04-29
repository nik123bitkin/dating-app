using System.Threading.Tasks;
using AppCore.DTOs;

namespace AppCore.Interfaces
{
    public interface IAuthService
    {
        Task<UserForDetailedDTO> Register(UserForRegisterDto userForRegisterDto);

        Task<object> Login(UserForLoginDto userForLoginDto);
    }
}
