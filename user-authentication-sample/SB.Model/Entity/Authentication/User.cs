using System.ComponentModel.DataAnnotations.Schema;

namespace SB.Model.Entity.Authentication
{
    public class User : AuditableEntity
    {
        [Column(Order = 1)]
        public string FirstName { get; set; }
        [Column(Order = 2)]
        public string LastName { get; set; }
        [Column(Order = 3)]
        public string Username { get; set; }
        [Column(Order = 4)]
        public virtual string Email { get; set; }
        [Column(Order = 5)]
        public virtual string Password { get; set; }
        [Column(Order = 6)]
        public byte[] PasswordHash { get; set; }
        [Column(Order = 7)]
        public byte[] PasswordSalt { get; set; }
        public virtual ProfilePhoto ProfilePhoto { get; protected set; }
    }
}
