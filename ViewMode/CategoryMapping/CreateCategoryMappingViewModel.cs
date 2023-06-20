using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CategoryMapping
{
    public class CreateCategoryMappingViewModel
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }

        public double ExperienceYear { get; set; }

        public string Introduction { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
