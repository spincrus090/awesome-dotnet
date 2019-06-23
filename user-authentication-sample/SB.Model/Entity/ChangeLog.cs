using SB.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SB.Model.Entity
{
    public class ChangeLog : BaseEntity
    {
        [Column(Order = 1)]
        public virtual string EntityName { get; set; }

        [Column(Order = 2)]
        public EntityOperation Operation { get; set; }

        [Column(Order = 3)]
        public virtual string PrimaryKey { get; set; }

        [Column(Order = 4)]
        public virtual string PropertyName { get; set; }

        [Column(Order = 5)]
        public virtual string OldValue { get; set; }

        [Column(Order = 6)]
        public virtual string NewValue { get; set; }

        [Column(Order = 7)]
        public virtual string ChangedBy { get; set; }

        [Column(Order = 8)]
        public virtual DateTime ChangedDate { get; set; }
    }
}
