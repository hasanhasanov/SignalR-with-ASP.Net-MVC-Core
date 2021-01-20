using System.Collections.Generic;
using chat.Models.Group;

namespace chat.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Groups = new List<GroupViewModel>();
        }
        
        public List<GroupViewModel> Groups { get; set; }
    }
}