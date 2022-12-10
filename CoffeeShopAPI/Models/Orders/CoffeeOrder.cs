using CoffeeShopAPI.Models.Ingredients;
using CoffeeShopAPI.Models.Products;
using CoffeeShopAPI.Models.Sizes;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Orders
{
    public class CoffeeOrder
    {
        // Database columns.
        [Key]
        public int CoffeeOrderId { get; set; }
        [Required(ErrorMessage = "Field CoffeId cannot be empty")]
        public int CoffeeId { get; set; }
        [Required(ErrorMessage = "Field CoffeSizeId cannot be empty")]
        public int CoffeeSizeId { get; set; }
        public int AlcoholId { get; set; }
        public int SyrupId { get; set; }
        public int MilkId { get; set; }
        public int SupplementsId { get; set; }
        [Required(ErrorMessage = "Field Quantity cannot be empty")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Field OrderId cannot be empty")]
        public int OrderId { get; set; }

        // Objects for relationships
        public virtual Coffee Coffee { get; set; }
        public virtual CoffeeSize CoffeeSize { get; set; }
        public virtual Alcohol Alcohol { get; set; }
        public virtual Syrup Syrup { get; set; }
        public virtual Milk Milk { get; set; }
        public virtual Supplements Supplements { get; set; }
        public virtual Order Order { get; set; }
    }
}
