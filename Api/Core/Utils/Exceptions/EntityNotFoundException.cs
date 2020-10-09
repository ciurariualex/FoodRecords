using System;

namespace Core.Utils.Exceptions
{
    public sealed class EntityNotFoundException<TEntity> : Exception
        where TEntity : class
    {
        private readonly string message = string.Empty;

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
        {
            this.message = message;
        }

        public sealed override string StackTrace => $"{ typeof(TEntity) } not found! { message }";

        public sealed override string Message => $"{ typeof(TEntity) } not found!";
    }
}
