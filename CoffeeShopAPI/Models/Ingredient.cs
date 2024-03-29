﻿using CoffeeShopAPI.Helpers.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class Ingredient
    {
        #region Database columns
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        public string IngredientType { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion
        #region Objects for relationships
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        #endregion
    }
}
