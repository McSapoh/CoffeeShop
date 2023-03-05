namespace CoffeeShopAPI.Helpers.DTO
{
    public class CodeForTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Code { get; set; }
        public string CodeVerifier { get; set; }
        public string RedirectUrl { get; set; }
        public string PostUrl { get; set; }
    }
}
