using SB.Enums;
using SB.Model.Entity.Interface;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SB.Model.Entity
{
    public class AuditableEntity : BaseEntity, IAuditableEntity
    {
        private StringBuilder _hmacStringBuilder;

        public AuditableEntity()
        {
            this._hmacStringBuilder = new StringBuilder();
        }

        [Column(Order = 91)]
        public DateTime CreatedDate { get; set; }
        [Column(Order = 92)]
        public EntityStatus Status { get; set; }
        [Column(Order = 93)]
        public string Hmac
        {
            get
            {
                this._hmacStringBuilder.Clear();
                Type type = this.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (var property in properties)
                {
                    // RowVersion, Hmac ve Navigation Property'leri hesaplanmamalı.
                    if (property.Name != "RowVersion" && property.Name != "Hmac" && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && !property.PropertyType.IsClass)
                    {
                        object value = property.GetValue(this);
                        this._hmacStringBuilder.Append(value);
                    }
                }

                // şifreleme için gerekli gizli anahtar. //TODO: ortak bir yere alınmalı
                byte[] key = Encoding.UTF8.GetBytes("A2?3R_tp*Olq/aX-1EE[2qa^lgH1rGF1z>ta!s-DR4%)");
                using (var hmac = new HMACSHA256(key))
                {
                    // Özelliklerin karma değerini hesaplar.
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(this._hmacStringBuilder.ToString()));
                    return Convert.ToBase64String(hash);
                }
            }

            private set
            {
                /*Entity Framework için gerekli boş setter.*/
            }
        }
    }
}
