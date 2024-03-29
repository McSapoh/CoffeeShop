﻿using CoffeeShopAPI.UnitOfWork.Repositories;
using System.Threading.Tasks;

namespace CoffeeShopAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IIngredientRepository IngredientRepository { get; set; }
        public IConfirmEmailTokenRepository ConfirmEmailTokenRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductOrderRepository ProductOrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ISizeRepository SizeRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public Task<bool> SaveAsync();
    }
}
