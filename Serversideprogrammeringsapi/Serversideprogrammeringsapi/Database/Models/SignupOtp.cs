using System.ComponentModel.DataAnnotations;

namespace Serversideprogrammeringsapi.Database.Models
{
    public class SignupOtp : IDatedEntity
    {

        /// <summary>
        /// The primary key alongside who the code was sent to as the username
        /// </summary>
        [Key]
        public string SentTo { get; set; }

        /// <summary>
        /// The number of failed attempts
        /// </summary>
        public int FailedAttempts { get; set; } = 0;

        /// <summary>
        /// When the code expires which will be stored in UTC 
        /// </summary>
        public DateTimeOffset Expiration { get; set; }

        /// <summary>
        /// Indicates whether or not the code has been validated, Default false
        /// </summary>
        public bool Validated { get; set; } = false;

        /// <summary>
        /// The code that is stored in the datbase
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// UID used for combining SignupOtp with an user
        /// </summary>
        public long? UId { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; }
    }
}
