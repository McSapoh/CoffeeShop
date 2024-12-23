﻿using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.UnitOfWork.Repositories
{
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(CoffeeShopContext db)
        {
            _dbSet = db.Set<Ingredient>();
        }
        public override Ingredient GetById(int Id) => _dbSet.FirstOrDefault(p => p.Id == Id);
        public async override Task<Ingredient> GetByIdAsync(int Id) => await _dbSet.FirstOrDefaultAsync(p => p.Id == Id);
        public PagedList<Ingredient> GetPagedList(PagingParameters pagingParameters, string type)
        {
            var items = _dbSet.Where(o => o.IngredientType == type && o.IsActive).ToList();
            var resultList = items.Skip(
                (pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize
            ).ToList();
            return new PagedList<Ingredient>(resultList, items.Count(), pagingParameters.PageSize, pagingParameters.PageSize);
        }
    }
}
