using CoffeeShopAPI.Helpers.Services;
using CoffeeShopAPI.Models.Sizes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Products
{
    public class Tea : Product
    {
        public override string ImagePath { get; set; } = "/Tea/DefaultTeaImage.png";
        public virtual ICollection<TeaSize> Sizes { get; set; }
    }
}
