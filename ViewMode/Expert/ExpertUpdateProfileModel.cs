using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Expert
{
    public class ExpertUpdateProfileModel
    {
        [Required]
        public string Fullname { get; set; } = null!;
        [Required]
        public string CerfificateLink { get; set; } = null!;
        [Required]
        public string Introduction { get; set; } = null!;
        [Required]
        public string WorlkRole { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
    }
}
