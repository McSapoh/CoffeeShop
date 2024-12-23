﻿using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.Ingredients;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Services;
using CoffeeShopAPI.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers.Ingredients
{
    [ApiExplorerSettings(IgnoreApi = true), Authorize]
    public class IngredientsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIngredientService _ingredientService;
        private readonly IMapper _mapper;
        protected readonly IngredientType _ingredientType;
        protected readonly ILogger<IngredientsController> _logger;

        public IngredientsController(IUnitOfWork unitOfWork, ILogger<IngredientsController> logger,
            IIngredientService ingredientService, IMapper mapper, IngredientType ingredientType)
        {
            _unitOfWork = unitOfWork;
            _ingredientService = ingredientService;
            _mapper = mapper;
            _ingredientType = ingredientType;
            _logger = logger;
        }

        /// <summary>
        /// Gets ingredient.
        /// </summary>
        /// <response code="200">Returns ingredient from db</response>
        /// <response code="400">If the id parameter is not int</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the ingredient from db is null</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"GET({id}) {this}.Get called.");
            var result = await _ingredientService.Get(id);
            _logger.LogInformation($"GET({id}) {this}.Get finished.");
            return result;
        }

        /// <summary>
        /// Gets paged list of ingredients.
        /// </summary>
        /// <response code="200">Returns paged list of ingredients</response>
        /// <response code="401">Unathorized</response>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Get([FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation($"GET {this}.Get called.");
            var objectsFromDb = _unitOfWork.IngredientRepository
                .GetPagedList(pagingParameters, _ingredientType.ToString());
            var metadata = PagedList<Ingredient>.GetMetadata(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            _logger.LogInformation($"GET {this}.Get finished.");
            return Ok(objectsFromDb);
        }

        /// <summary>
        /// Creates ingredient.
        /// </summary>
        /// <response code="201">If the ingredient successfully created</response>
        /// <response code="400">If model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="500">If unknown error occurred while creating</response>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] EditIngredientDTO objectFromPage)
        {
            _logger.LogInformation($"POST {this}.Create called.");

            // Validation.
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid");
                return BadRequest(ModelState);
            }

            // Action.
            var ingredient = _mapper.Map<Ingredient>(objectFromPage);
            ingredient.IngredientType = _ingredientType.ToString();
            var result = await _ingredientService.Create(ingredient);
            _logger.LogInformation($"POST {this}.Create finished.");
            return result;
        }

        /// <summary>
        /// Updates ingredient.
        /// </summary>
        /// <response code="201">If the ingredient successfully updated</response>        
        /// <response code="400">If model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the ingredient from db is null</response>
        /// <response code="409">If the ingredient from db has different IngredientType from ingredient mapped in controller</response>
        /// <response code="500">If unknown error occurred while updating</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromForm] EditIngredientDTO objectFromPage)
        {
            _logger.LogInformation($"PUT {this}.Update called.");

            // Validation.
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid");
                return BadRequest(ModelState);
            }

            // Action.
            var ingredient = _mapper.Map<Ingredient>(objectFromPage);
            ingredient.Id = id;
            ingredient.IngredientType = _ingredientType.ToString();
            var result = await _ingredientService.Update(ingredient);
            _logger.LogInformation($"PUT {this}.Update finished.");
            return result;
        }

        /// <summary>
        /// Deletes ingredient.
        /// </summary>
        /// <response code="200">If the ingredient successfully deleted</response>
        /// <response code="400">If the id parameter is not int</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the ingredient from db is null</response>
        /// <response code="409">If the ingredient is already deleted</response>
        /// <response code="500">If unknown error occurred while deleting</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"DELETE {this}.DELETE called.");
            var result = await _ingredientService.Delete(id);
            _logger.LogInformation($"DELETE {this}.DELETE finished.");
            return result;
        }
    }
}
