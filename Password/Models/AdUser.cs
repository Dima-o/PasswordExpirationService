using System;

namespace Password.Models
{
    internal class AdUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string DistinguishedName { get; set; }

        public DateTime PasswordLastSet { get; set; }
    }
}
