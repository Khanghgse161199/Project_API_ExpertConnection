using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CategoryMapping;
using ViewModel.Rating;

namespace ViewModel.Expert
{
    public class ExpertProfileModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Fullname { get; set; } = null!;

        public string CerfificateLink { get; set; } = null!;

        public string Introduction { get; set; } = null!;

        public double RatingSummary { get; set; }

        public string WorlkRole { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public List<CategoryMappingProfileModel> listCategoryMapping { get; set; } = null!;
    }
}
