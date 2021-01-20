using System.Collections.Generic;

namespace chat.Data.Entities
{
    public class User : BaseEntity
    {
        private ICollection<Conversation> _conversations;

        public string Name { get; set; }
        public int[] Groups { get; set; }

        public virtual ICollection<Conversation> Conversations
        {
            get => _conversations ?? (_conversations = new List<Conversation>());
            set => _conversations = value;
        }
    }
}