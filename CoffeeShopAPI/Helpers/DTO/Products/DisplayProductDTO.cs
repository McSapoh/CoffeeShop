﻿using CoffeeShopAPI.Models;
using System.Collections.Generic;

namespace CoffeeShopAPI.Helpers.DTO.Products
{
    public class DisplayProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}
