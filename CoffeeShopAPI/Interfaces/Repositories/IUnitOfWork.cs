using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        #region Ingredient Repositories.
        public IAlcoholRepository AlcoholRepository { get; set; }
        public IMilkRepository MilkRepository { get; set; }
        public ISauceRepository SauceRepository { get; set; }
        public ISupplementsRepository SupplementsRepository { get; set; }
        public ISyrupRepository SurypRepository { get; set; }
        #endregion
        #region Order Repositories.
        public ICoffeeOrderRepository CoffeeOrderRepository { get; set; }
        public IDessertOrderRepository DessertOrderRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public ISandwichOrderRepository SandwichOrderRepository { get; set; }
        public ISnackOrderRepository SnackOrderRepository { get; set; }
        public ITeaOrderRepository TeaOrderRepository { get; set; }
        #endregion
        #region Product Repositories.
        public ICoffeeRepository CoffeeRepository { get; set; }
        public IDessertRepository DessertRepository { get; set; }
        public ISandwichRepository SandwichRepository { get; set; }
        public ISnackRepository SnackRepository { get; set; }
        public ITeaRepository TeaRepository { get; set; }
        #endregion
        #region Product Size Repositories.
        public ICoffeeSizeRepository CoffeeSizeRepository { get; set; }
        public IDessertSizeRepository DessertSizeRepository { get; set; }
        public ISandwichSizeRepository SandwichSizeRepository { get; set; }
        public ISnackSizeRepository SnackSizeRepository { get; set; }
        public ITeaSizeRepository TeaSizeRepository { get; set; }
        #endregion
        public IUserRepository UserRepository { get; set; }
        public Task<bool> SaveAsync();
    }
}
