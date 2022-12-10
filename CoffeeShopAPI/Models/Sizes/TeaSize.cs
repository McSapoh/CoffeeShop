using CoffeeShopAPI.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Models.Sizes
{
    public class TeaSize
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public double Price { get; set; }
        public int ProductId { get; set; }

        public virtual Tea Tea { get; set; }
    }
}
