﻿using System;

namespace Core.Common.Api.Registration
{
    public class RegistrationView
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string IsActive { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}

