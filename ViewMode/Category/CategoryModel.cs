using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Category
{
    public class CategoryModel
    {   
        [Required]
        public string Name { get; set; } = null!;     
    }
}
