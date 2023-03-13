namespace Serversideprogrammeringsapi.Identity.Properties
{
    public class OTPOptions
    {
        /// <summary>
        /// Setting for the number of failed attempts
        /// </summary>
        public int AttemptsLimit = 5;

        /// <summary>
        /// Setting for how long the OTP should be valid for
        /// </summary>
        public double OTPValidFor = 30;
    }
}
