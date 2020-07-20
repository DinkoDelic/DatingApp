using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    //Data tranfer object, used to pass information from SPA to API when we're creating a new user
    //UserForRegisterDto.Username becomes User.Username and -||-.Password becomes User.Password
    
    //We are gonna validate here instead of User class because we need to validate username and pass
    public class UserForRegisterDto
    {
        [Required] //Data annotation used to validate a property(emails, phones...) with regex
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength= 4, ErrorMessage = "Password must be between 4 and 8 characters.")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}