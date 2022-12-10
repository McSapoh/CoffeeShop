using CoffeeShopAPI.Models.Ingredients;
using CoffeeShopAPI.Models.Products;
using CoffeeShopAPI.Models.Sizes;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Orders
{
    public class SnackOrder
    {
        // Database columns.
        [Key]
        public int SnackOrderId { get; set; }
        [Required(ErrorMessage = "Field SnackId cannot be empty")]
        public int SnackId { get; set; }
        [Required(ErrorMessage = "Field SnackSizeId cannot be empty")]
        public int SnackSizeId { get; set; }
        public int SauceId { get; set; }
        public int SupplementsId { get; set; }
        [Required(ErrorMessage = "Field Quantity cannot be empty")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Field OrderId cannot be empty")]
        public int OrderId { get; set; }

        // Objects for relationships
        public virtual Snack Snack { get; set; }
        public virtual SnackSize SnackSize { get; set; }
        public virtual Sauce Sauce { get; set; }
        public virtual Supplements Supplements { get; set; }
        public virtual Order Order { get; set; }
    }
}
