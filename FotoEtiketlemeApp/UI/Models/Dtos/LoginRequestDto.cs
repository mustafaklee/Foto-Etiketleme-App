using System.ComponentModel.DataAnnotations;

namespace UI.Models.Dtos
{
    public class LoginRequestDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
