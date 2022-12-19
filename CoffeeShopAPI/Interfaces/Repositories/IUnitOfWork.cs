using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public IIngredientRepository IngredientRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductOrderRepository ProductOrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ISizeRepository SizeRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public Task<bool> SaveAsync();
    }
}
