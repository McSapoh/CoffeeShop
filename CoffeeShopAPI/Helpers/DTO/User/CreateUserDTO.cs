using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Helpers.DTO.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Please enter Your Name")]
        public string Name { get; set; }
        [EmailAddress, Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Your adress")]
        public string Adress { get; set; }
        [MinLength(4), Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
    }
}
