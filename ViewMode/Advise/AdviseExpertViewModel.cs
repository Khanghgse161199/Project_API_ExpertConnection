using DatabaseConection.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Advise
{
    public class AdviseExpertViewModel
    {
        public string IdAdvise {get; set;}
        public string NameCategoryMapping { get; set; }
        public string NameExpert { get; set; }
        public string CreateDay { get; set; }
    }
}
