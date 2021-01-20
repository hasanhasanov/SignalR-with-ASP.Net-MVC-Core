namespace Chat.Models
{
    using System.Collections.Generic;
    using Chat.Models.Group;
    using Chat.Models.User;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            Groups = new List<GroupViewModel>();
            Users = new List<UserViewModel>();
        }

        public List<GroupViewModel> Groups { get; set; }
        public List<UserViewModel> Users { get; set; }
        public int UserId { get; set; }
    }
}