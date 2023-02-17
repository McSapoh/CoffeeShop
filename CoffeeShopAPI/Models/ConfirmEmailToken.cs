using System;

namespace CoffeeShopAPI.Models
{
    public class ConfirmEmailToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
