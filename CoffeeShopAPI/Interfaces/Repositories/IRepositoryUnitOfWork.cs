using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IRepositoryUnitOfWork
    {
        #region Ingredient Repositories.
        public IAlcoholRepository AlcoholRepository { get;}
        public IMilkRepository MilkRepository { get; }
        public ISauceRepository SauceRepository { get; }
        public ISupplementsRepository SupplementsRepository { get; }
        public ISyrupRepository SurypRepository { get; }
        #endregion
        #region Order Repositories.
        public ICoffeeOrderRepository CoffeeOrderRepository { get; }
        public IDessertOrderRepository DessertOrderRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public ISandwichOrderRepository SandwichOrderRepository { get; }
        public ISnackOrderRepository SnackOrderRepository { get; }
        public ITeaOrderRepository TeaOrderRepository { get; }
        #endregion
        #region Product Repositories.
        public ICoffeeRepository CoffeeRepository { get; }
        public IDessertRepository DessertRepository { get; }
        public ISandwichRepository SandwichRepository { get; }
        public ISnackRepository SnackRepository { get; }
        public ITeaRepository TeaRepository { get; }
        #endregion
        #region Product Size Repositories.
        public ICoffeeSizeRepository CoffeeSizeRepository { get; }
        public IDessertSizeRepository DessertSizeRepository { get; }
        public ISandwichSizeRepository SandwichSizeRepository { get; }
        public ISnackSizeRepository SnackSizeRepository { get; }
        public ITeaSizeRepository TeaSizeRepository { get; }
        #endregion
        public IUserRepository UserRepository { get; }
        public Task<bool> SaveAsync();
    }
}
