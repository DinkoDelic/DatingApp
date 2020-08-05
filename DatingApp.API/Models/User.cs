using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        
        public byte[] PasswordHash { get; set; }

        //Additional secturity feature to generate unique hashes for each password
        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        // EF doesn't support many to many relationship so we must implement two one to many relationships
        public ICollection<Like> Likers { get; set; }

        public ICollection<Like> Likees { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        
    }
}