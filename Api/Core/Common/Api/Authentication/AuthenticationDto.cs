using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.Authentication
{
    public class AuthenticationDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
