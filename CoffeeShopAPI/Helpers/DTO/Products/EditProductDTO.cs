using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Helpers.DTO.Products
{
    public class EditProductDTO
    {
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        [MinLength(1), MaxLength(5)]
        public virtual ICollection<SizeDTO> Sizes { get; set; }
    }
}
