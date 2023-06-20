using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.User
{
    public class UserUpdateProfileModel
    {
        public string FullName { get; set; } = null!;

        public string Birthday { get; set; } = null!;

        public string Address { get; set; } = null!;
      
        public string Introduction { get; set; } = null!;
  
        public string PhoneNumber { get; set; } = null!;
       
        public string Email { get; set; } = null!;
    }
}
