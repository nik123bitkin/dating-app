using System.Threading.Tasks;
using AppCore.DTOs;

namespace AppCore.Interfaces
{
    public interface IAuthService
    {
        Task<UserForDetailedDto> RegisterAsync(UserForRegisterDto userForRegisterDto);

        Task<object> LoginAsync(UserForLoginDto userForLoginDto);
    }
}
