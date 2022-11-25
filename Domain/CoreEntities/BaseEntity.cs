using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.CoreEntities
{
    public class BaseEntity
    {
        [Key]
        public virtual long Id { get; set; }

        [Required]
        public virtual DateTime CreationTs { get; set; }

    }
}
