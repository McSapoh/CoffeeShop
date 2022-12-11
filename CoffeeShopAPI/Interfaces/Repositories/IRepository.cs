using System.Collections.Generic;

namespace CoffeeShopAPI.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        public T GetById(int Id);
        public IEnumerable<T> GetAll();
        public void Create(T Item);
        public void Update(T Item);
        public void Delete(int Id);
        public void Delete(T Item);
    }
}

