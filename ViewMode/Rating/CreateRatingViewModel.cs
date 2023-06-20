using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Rating
{
    public class CreateRatingViewModel
    {
        [Required]
        public string IdAdvise { get; set; }
        [Required]
        public float RatingPoint { get; set; }
        [Required]
        [MaxLength(400)]
        [DataType(DataType.Text)]
        public string Comment { get; set; } 
    }
}
