using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Helpers.DTO.Ingredients
{
    public class EditIngredientDTO
    {
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
