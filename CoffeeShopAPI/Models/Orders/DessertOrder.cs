using CoffeeShopAPI.Models.Ingredients;
using CoffeeShopAPI.Models.Products;
using CoffeeShopAPI.Models.Sizes;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Orders
{
    public class DessertOrder
    {
        // Database columns.
        [Key]
        public int DessertOrderId { get; set; }
        [Required(ErrorMessage = "Field DessertId cannot be empty")]
        public int DessertId { get; set; }
        [Required(ErrorMessage = "Field DessertSizeId cannot be empty")]
        public int DessertSizeId { get; set; }
        public int SyrupId { get; set; }
        public int SupplementsId { get; set; }
        [Required(ErrorMessage = "Field Quantity cannot be empty")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Field OrderId cannot be empty")]
        public int OrderId { get; set; }

        // Objects for relationships
        public virtual Dessert Dessert { get; set; }
        public virtual DessertSize DessertSize { get; set; }
        public virtual Syrup Syrup { get; set; }
        public virtual Supplements Supplements { get; set; }
        public virtual Order Order { get; set; }
    }
}
