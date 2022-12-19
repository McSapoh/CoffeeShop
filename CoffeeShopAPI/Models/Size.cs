using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class Size
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

        public virtual Product Product { get; set; }
    }
}
