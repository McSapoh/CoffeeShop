namespace CoffeeShopAPI.Helpers.DTO
{
    public class ExternalAuthorizationRequest
    {
        public string ClientId { get; set; }
        public string RedirectUrl { get; set; }
        public string Scope { get; set; }
        public string CodeChallenge { get; set; }
        public string Uri { get; set; }
    }
}
