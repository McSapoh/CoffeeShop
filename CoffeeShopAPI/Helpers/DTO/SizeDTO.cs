using CoffeeShopAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Helpers.DTO
{
    public class SizeDTO
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public double Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
