using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Rating
{
    public class RatingViewModel
    {
        public string IdRating { get; set; }    
        public string FromUser { get; set; }
        public string ToCategoryMapping { get; set; }
        public double RatingPoint { get; set; }
        public string comment { get; set; }
    }
}
