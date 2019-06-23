using SB.Enums;
using System;

namespace SB.Model.Entity.Interface
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        EntityStatus Status { get; set; }
        string Hmac { get; }
    }
}
