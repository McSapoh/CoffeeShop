using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Helpers.DTO.User
{
    public class EditUserDTO
    {
        [Required(ErrorMessage = "Please enter Your Name")]
        public string Name { get; set; }
        [EmailAddress, Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }
        public string Adress { get; set; }
        [MinLength(4), Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
        public IFormFile Photo { get; set; }
    }
}
