using Core.Data.NotConfigured;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.User
{
    public class UserEdit
    {
        [Required]
        public Guid Id { get; set; } = default;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        public Settings Settings { get; set; }

        public Contact Contact { get; set; }
    }
}
