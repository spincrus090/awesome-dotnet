using SB.Model.Entity.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SB.Model.Entity
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Column(Order = 0)]
        public virtual Guid Id { get; set; }
    }
}
