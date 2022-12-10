using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Coffee 
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        public string ImagePath { get; set; } = "/Coffee/DefaultCoffeeImage";
        public bool IsActive { get; set; } = true;

        public virtual ICollection<CoffeeSize> Sizes { get; set; }
    }
}
