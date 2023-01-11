using AutoMapper;
using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Models;

namespace CoffeeShopAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Products
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            #endregion
            #region Ingredients
            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<IngredientDTO, Ingredient>();
            #endregion
            #region Users
            CreateMap<User, CreateUserDTO>();
            CreateMap<CreateUserDTO, User>();
            #endregion
        }
    }
}
