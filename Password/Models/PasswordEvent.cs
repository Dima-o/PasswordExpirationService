using System;

namespace Password.Models
{
    internal class PasswordEvent
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string EventType { get; set; }

        public DateTime EventDate { get; set; }

        public string NewPassword { get; set; }
    }
}
