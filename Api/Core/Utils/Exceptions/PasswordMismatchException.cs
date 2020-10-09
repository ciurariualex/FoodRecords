using System;

namespace Core.Utils.Exceptions
{
    public sealed class PasswordMismatchException : Exception
    {
        private readonly string message = string.Empty;

        public PasswordMismatchException()
        {
        }

        public PasswordMismatchException(string message)
        {
            this.message = message;
        }

        public sealed override string StackTrace => $"New password and the verification one are not the same! { message }";

        public sealed override string Message => "New password and the verification one are not the same!";
    }
}
