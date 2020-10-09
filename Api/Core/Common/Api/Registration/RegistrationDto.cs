using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.Registration
{
    public class RegistrationDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

