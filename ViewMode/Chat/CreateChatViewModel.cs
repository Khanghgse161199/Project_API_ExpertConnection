using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Chat
{
    public class CreateChatViewModel
    {
        [Required]
        public string IdAvise { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; } = null!;
        [Required, MinLength(0)]
        [DataType(DataType.Text)]
        public string ContentChat { get; set; } = null!;
    }
}
