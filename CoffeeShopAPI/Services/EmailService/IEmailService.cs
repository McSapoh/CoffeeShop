using CoffeeShopAPI.Helpers.DTO.Email;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Services
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(EmailDTO emailDTO);
    }
}
