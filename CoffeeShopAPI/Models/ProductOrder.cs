using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class ProductOrder
    {
        // Database columns.
        [Key]
        public int ProductOrderId { get; set; }
        [Required(ErrorMessage = "Field ProductId cannot be empty")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Field SizeId cannot be empty")]
        public int SizeId { get; set; }
        public int IngredientId { get; set; }
        [Required(ErrorMessage = "Field Quantity cannot be empty")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Field OrderId cannot be empty")]
        public int OrderId { get; set; }

        // Objects for relationships
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual Order Order { get; set; }
    }
}
