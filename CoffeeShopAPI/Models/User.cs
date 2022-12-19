﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Your Name")]
        public string Name { get; set; }
        [Phone, Required(ErrorMessage = "Please enter Your phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter Your adress")]
        public string Adress { get; set; }
        public string ImagePath { get; set; } = "/User/DefaultUserImage";
        public DateTime RegistrationDate { get; set; }
        [MinLength(4), Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
        public string Role { get; set; } = "User";


        public virtual ICollection<Order> Orders { get; set; }
    }
}
