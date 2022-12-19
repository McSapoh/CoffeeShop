using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Models;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeShopContext _db;
        public IUserRepository UserRepository { get; set; }
        public IIngredientRepository IngredientRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductOrderRepository ProductOrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ISizeRepository SizeRepository { get; set; }

        public async Task<bool> SaveAsync() => await _db.SaveChangesAsync() > 0;

        // Need to change after creating repository classes.
        public UnitOfWork (CoffeeShopContext context)
        {
            _db = context;
            IngredientRepository = new IngredientRepository(context);
            OrderRepository = new OrderRepository(context);
            ProductOrderRepository = new ProductOrderRepository(context);
            ProductRepository = new ProductRepository(context);
            SizeRepository = new SizeRepository(context);
            UserRepository = new UserRepository(context);
        }
    }
}
