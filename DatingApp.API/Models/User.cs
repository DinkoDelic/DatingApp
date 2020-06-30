namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        
        public byte[] PasswordHash { get; set; }

        //Additional secturity feature to generate unique hashes for each password
        public byte[] PasswordSalt { get; set; }
    }
}