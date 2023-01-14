using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Helpers.DTO.User
{
    public class UpdateUserDTO
    {
        [Key]
        public int Id { get; set; }
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
