using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

#nullable disable

namespace WebApi.Models
{
    public partial class User
    {
        public User()
        {
            Cases = new HashSet<Case>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] UserHash { get; set; }
        public byte[] UserSalt { get; set; }

        [JsonIgnore]
        public virtual ICollection<Case> Cases { get; set; }

        public void GeneratePassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                UserHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                UserSalt = hmac.Key;

            }
        }

        public bool ValidatePassword(string password)
        {
            using (var hmac = new HMACSHA512(UserSalt))
            {
                var computedUserHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i=0; i < computedUserHash.Length; i++)
                {
                    if (computedUserHash[i] != UserHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
