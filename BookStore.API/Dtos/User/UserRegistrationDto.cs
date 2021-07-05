using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Dtos.User
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}