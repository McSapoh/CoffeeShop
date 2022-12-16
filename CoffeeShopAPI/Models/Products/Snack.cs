using CoffeeShopAPI.Helpers.Services;
using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Snack : Product
    {
        public override string ImagePath { get; set; } = "/Snack/DefaultSnackImage.png";

        public virtual ICollection<SnackSize> Sizes { get; set; }
    }
}
