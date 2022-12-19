using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class Product
    {
        #region Database columns.
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        public string ProductType { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion
        #region Objects for relationships
        public virtual ICollection<Size> Sizes { get; set; }
        #endregion
        #region Methods
        #endregion
    }
}
