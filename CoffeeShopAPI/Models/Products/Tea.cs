using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Tea
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        public string ImagePath { get; set; } = "/Tea/DefaultTeaImage";
        public bool IsActive { get; set; } = true;

        public virtual ICollection<TeaSize> Sizes { get; set; }
    }
}
