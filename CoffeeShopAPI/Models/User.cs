using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public partial class User
    {
        #region Database columns
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Your Name")]
        public string Name { get; set; }
        [EmailAddress, Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }
        public string Adress { get; set; }
        public string ImagePath { get; set; } = "/User/DefaultUserImage";
        public bool IsConfirmed { get; set; } = false;
        public DateTime RegistrationDate { get; set; }
        public string Password { get; set; }
        #endregion
        #region Objects for relationships
        public virtual ICollection<Order> Orders { get; set; }
        public virtual RefreshToken RefreshToken { get; set; }
        public virtual ConfirmEmailToken ConfirmEmailToken { get; set; }
        #endregion
    }
}
