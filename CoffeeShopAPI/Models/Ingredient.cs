using CoffeeShopAPI.Helpers.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class Ingredient
    {
        #region Database columns
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        public string IngredientType { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion
        #region Objects for relationships
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        #endregion
        #region Methods
        public static Ingredient GetByDTO(IngredientDTO dto, IngredientType ingredientType)
        {
            Ingredient product = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                IngredientType = ingredientType.ToString(),
                IsActive = dto.IsActive,
            };
            return product;
        }
        #endregion
    }
}
