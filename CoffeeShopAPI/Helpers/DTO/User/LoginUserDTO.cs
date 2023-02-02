using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Helpers.DTO.User
{
    public class LoginUserDTO
    {
        [EmailAddress, Required(ErrorMessage = "Please enter Your phone number")]
        public string Email { get; set; }
        [MinLength(4), Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
    }
}
