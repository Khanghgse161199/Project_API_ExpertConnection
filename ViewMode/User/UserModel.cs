using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.User
{
    public class UserModel
    {

        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public string Birthday { get; set; }
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string Introduction { get; set; } = null!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!; 
    }
}
