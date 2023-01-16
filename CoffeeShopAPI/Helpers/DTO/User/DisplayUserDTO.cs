using System;

namespace CoffeeShopAPI.Helpers.DTO.User
{
    public class DisplayUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string ImagePath { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
