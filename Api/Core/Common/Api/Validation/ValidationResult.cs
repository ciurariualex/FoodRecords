using Core.Common.Api.User;

namespace Core.Common.Api.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public UserView User { get; set; }
    }
}