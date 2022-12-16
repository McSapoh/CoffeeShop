using CoffeeShopAPI.Helpers.Services;
using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Sandwich : Product
    {
        public override string ImagePath { get; set; } = "/Sandwich/DefaultSandwichImage.png";
        public virtual ICollection<SandwichSize> Sizes { get; set; }
    }
}
