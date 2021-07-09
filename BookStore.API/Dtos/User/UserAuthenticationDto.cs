using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Dtos.User
{
    public class UserAuthenticationDto
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string Password { get; set; }
    }
}