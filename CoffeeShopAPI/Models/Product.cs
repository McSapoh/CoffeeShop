using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class Product
    {
        // Database columns.
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        public ProductType ProductType { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; } = true;

        // Objects for relationships
        public virtual User User { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}
