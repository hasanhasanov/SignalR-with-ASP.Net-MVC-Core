using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Data.Entities
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }

        [Column(Order = 1)]
        public bool IsActive { get; set; }

        [Column(Order = 2)]
        public bool IsDeleted { get; set; }

        [Column(Order = 3)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 4)]
        public DateTime? UpdatedDate { get; set; }
    }
}