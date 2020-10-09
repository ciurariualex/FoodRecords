using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.User
{
    public class UserAdd
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Username { get; set; }
    }
}