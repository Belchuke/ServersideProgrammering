namespace Serversideprogrammeringsapi.Models
{
    public class OtpValidationInput
    {
        public string Username { get; set; }

        public string Code { get; set; }

        [GraphQLIgnore]
        public long? UserId { get; set; }
    }
}
