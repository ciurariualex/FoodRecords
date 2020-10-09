using Core.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.User
{
    public class UserRoleEdit
    {
        [Required]
        public Guid Id { get; set; } = default;

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Surname { get; set; }

        public Role Role { get; set; }
    }
}
