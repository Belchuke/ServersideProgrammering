﻿namespace Serversideprogrammeringsapi.Models
{
    public class RegisterResult
    {
        public bool IsSuccessful { get; set; }

        public DateTimeOffset? Expires { get; set; }

        public string Message { get; set; }
    }
}
