using CoffeeShopAPI.Helpers.Services;
using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Coffee : Product
    {
        public override string ImagePath { get; set; } = "/Coffee/DefaultCoffeeImage.png";
        public virtual ICollection<CoffeeSize> Sizes { get; set; }
    }
}
