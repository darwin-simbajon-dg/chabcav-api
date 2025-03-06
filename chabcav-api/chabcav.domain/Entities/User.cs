using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Role { get; set; }


        public User()
        {
            
        }

        public User(string username, string email, string passwordHash, string role)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            Role = role;
        }

        public User(Guid id, string username, string email, string passwordHash, string role)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            Role = role;
        }

        public User(string username, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }
    }
   
}


