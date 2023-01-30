using AutoMapper;
using CoffeeShopAPI.Helpers.DTO;
using CoffeeShopAPI.Helpers.DTO.Ingredients;
using CoffeeShopAPI.Helpers.DTO.Products;
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
            CreateMap<Product, EditProductDTO>();
            CreateMap<EditProductDTO, Product>();
            #endregion
            #region Sizes
            CreateMap<Size, SizeDTO>();
            CreateMap<SizeDTO, Size>();
            #endregion
            #region Ingredients
            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<IngredientDTO, Ingredient>();
            CreateMap<Ingredient, EditIngredientDTO>();
            CreateMap<EditIngredientDTO, Ingredient>();
            #endregion
            #region Users
            CreateMap<User, CreateUserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<User, UpdateUserDTO>();
            CreateMap<UpdateUserDTO, User>();
            CreateMap<User, DisplayUserDTO>();
            CreateMap<DisplayUserDTO, User>();
            #endregion
        }
    }
}
