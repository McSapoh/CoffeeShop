using CoffeeShopAPI.Helpers.DTO;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public class Size
    {
        #region Database columns
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Description cannot be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public double Price { get; set; }
        public int ProductId { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion
        #region Objects for relationships
        public virtual Product Product { get; set; }
        #endregion
        #region Methods
        public static Size GetByDTO(SizeDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            IsActive = dto.IsActive,
        };
        #endregion
    }
}
