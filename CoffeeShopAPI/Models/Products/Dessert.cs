using CoffeeShopAPI.Helpers.Services;
using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Dessert : Product
    {
        public override string ImagePath { get; set; } = "/Dessert/DefaultDessertImage.png";
        public virtual ICollection<DessertSize> Sizes { get; set; }
    }
}
