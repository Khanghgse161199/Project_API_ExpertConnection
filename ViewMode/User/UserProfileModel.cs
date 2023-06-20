using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.User
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; } = null!;

        public DateTime Birthday { get; set; }

        public string Address { get; set; } = null!;

        public string Introduction { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool EmailActivated { get; set; }

        public bool UserConfirm { get; set; }
        public bool IsActive { get; set; }
    }
}
