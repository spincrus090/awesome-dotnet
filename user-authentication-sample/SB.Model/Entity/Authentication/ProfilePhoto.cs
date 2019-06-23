using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SB.Model.Entity.Authentication
{
    public class ProfilePhoto : AuditableEntity
    {
        [Column(Order = 1)]
        public virtual byte[] Photo { get; set; }
        [Column(Order = 2)]
        public virtual Guid UserId { get; set; }

        public virtual User User { get; protected set; }
    }
}
