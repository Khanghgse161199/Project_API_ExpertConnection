using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Rating;

namespace ViewModel.CategoryMapping
{
    public class CategoryMappingProfileModel
    {
        public string IdCategoryMapping { get; set; }
        public string NameOfCategoryMapping { get; set; }
        public double SummaryRating { get; set; }

        public List<RatingInCategoryMappingViewModel> ratingViewModels;
    }
}
