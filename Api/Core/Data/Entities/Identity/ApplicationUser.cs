using Core.Data.Entities.Base;
using Core.Data.Enums;
using System;
using System.Collections.Generic;

namespace Core.Data.Entities.Identity
{
    public class ApplicationUser : BaseEntity<Guid>
    {
        public string ContactJson { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string SettingsJson { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }

        public void SetHashAndSalt(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}

