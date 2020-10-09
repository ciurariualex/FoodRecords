namespace Core.Common.Api.Password
{
    public class SecuredPassword
    {
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}