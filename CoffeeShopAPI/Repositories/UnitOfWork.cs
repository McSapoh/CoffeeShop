using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Interfaces.Repositories.Sizes;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Repositories.Ingredients;
using CoffeeShopAPI.Repositories.Orders;
using CoffeeShopAPI.Repositories.Products;
using CoffeeShopAPI.Repositories.Sizes;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeShopContext _db;
        public IUserRepository UserRepository { get; set; }
        #region Ingredient Repositories.
        public IAlcoholRepository AlcoholRepository { get; set; }
        public IMilkRepository MilkRepository { get; set; }
        public ISauceRepository SauceRepository { get; set; }
        public ISupplementsRepository SupplementsRepository { get; set; }
        public ISyrupRepository SyrupRepository { get; set; }
        #endregion
        #region Order Repositories.
        public Interfaces.Repositories.Orders.ICoffeeOrderRepository CoffeeOrderRepository { get; set; }
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
        public async Task<bool> SaveAsync() => await _db.SaveChangesAsync() > 0;

        // Need to change after creating repository classes.
        public UnitOfWork (CoffeeShopContext context)
        {
            _db = context;
            UserRepository = new UserRepository(context);
            #region Ingredient Repositories.
            AlcoholRepository = new AlcoholRepository(context);
            MilkRepository = new MilkRepository(context);
            SauceRepository = new SauceRepository(context);
            SupplementsRepository = new SupplementsRepository(context);
            SyrupRepository = new SyrupRepository(context);
            #endregion
            #region Order Repositories.
            CoffeeOrderRepository = new CoffeeOrderRepository(context);
            DessertOrderRepository = new DessertOrderRepository(context);
            OrderRepository = new OrderRepository(context);
            SandwichOrderRepository = new SandwichOrderRepository(context);
            SnackOrderRepository = new SnackOrderRepository(context);
            TeaOrderRepository = new TeaOrderRepository(context);
            #endregion
            #region Product Repositories.
            CoffeeRepository = new CoffeeRepository(context);
            DessertRepository = new DessertRepository(context);
            SandwichRepository = new SandwichRepository(context);
            SnackRepository = new SnackRepository(context);
            TeaRepository = new TeaRepository(context);
            #endregion
            #region Product Size Repositories.
            CoffeeSizeRepository = new CoffeeSizeRepository(context);
            DessertSizeRepository = new DessertSizeRepository(context);
            SandwichSizeRepository = new SandwichSizeRepository(context);
            SnackSizeRepository = new SnackSizeRepository(context);
            TeaSizeRepository = new TeaSizeRepository(context);
            #endregion
        }
    }
}
