using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Chat
{
    public class ChatViewModel
    {
        public string Id { get; set; } = null!;
        public string FromAcc { get; set; } = null!;
        public string ToAc { get; set; } = null!;     
        public string ImageUrl { get; set; } = null!;
        public string ContentChat { get; set; } = null!;
        public DateTime CreateDate { get; set; }
    }
}
