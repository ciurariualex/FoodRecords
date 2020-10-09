using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.Validation
{
    public class ValidationDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}