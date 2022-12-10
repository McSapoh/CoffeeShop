using CoffeeShopAPI.Models.Ingredients;
using CoffeeShopAPI.Models.Products;
using CoffeeShopAPI.Models.Sizes;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Orders
{
    public class TeaOrder
    {
        // Database columns.
        [Key]
        public int TeaOrderId { get; set; }
        [Required(ErrorMessage = "Field TeaId cannot be empty")]
        public int TeaId { get; set; }
        [Required(ErrorMessage = "Field TeaSizeId cannot be empty")]
        public int TeaSizeId { get; set; }
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
        public virtual Tea Tea { get; set; }
        public virtual TeaSize TeaSize { get; set; }
        public virtual Syrup Syrup { get; set; }
        public virtual Milk Milk { get; set; }
        public virtual Supplements Supplements { get; set; }
        public virtual Order Order { get; set; }
    }
}
