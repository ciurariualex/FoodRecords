using Core.Data.NotConfigured;
using Core.Utils.Automapper.Interfaces;
using System;

namespace Core.Common.Api.User
{
    public class UserView : IUserViewable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public Settings Settings { get; set; }
        public Contact Contact { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
