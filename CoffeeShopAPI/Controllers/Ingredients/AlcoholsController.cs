﻿using AutoMapper;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoffeeShopAPI.Controllers.Ingredients
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Ingredients")]
    public class AlcoholsController : IngredientsController
    {
        public AlcoholsController(IUnitOfWork unitOfWork, ILogger<IngredientsController> logger, 
            IIngredientService ingredientService, IMapper mapper) : 
            base(unitOfWork, logger, ingredientService, mapper, IngredientType.alcohol)
        { }
    }
}
