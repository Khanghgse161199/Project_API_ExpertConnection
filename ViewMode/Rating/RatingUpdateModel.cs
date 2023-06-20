using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Rating
{
    public class RatingUpdateModel
    {
        [Required]
        public string newComment { get; set; }
        [Required]  
        public double newPoint { get; set; }
    }
}
