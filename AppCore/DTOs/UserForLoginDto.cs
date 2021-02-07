using System.ComponentModel.DataAnnotations;

namespace AppCore.DTOs
{
    public class UserForLoginDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
