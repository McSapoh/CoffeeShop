using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.Products;
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

namespace CoffeeShopAPI.Controllers.Products
{
    [ApiExplorerSettings(IgnoreApi = true), Authorize]
    public class ProductsController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IProductService _productService;
        protected readonly IMapper _mapper;
        protected readonly ProductType _productType;
        protected readonly ILogger<ProductsController> _logger;

        public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger,
            IProductService productService, IMapper mapper, ProductType productType)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _mapper = mapper;
            _productType = productType;
            _logger = logger;
        }

        /// <summary>
        /// Gets paged list of products.
        /// </summary>
        /// <response code="200">Returns paged list of porudcts</response>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation($"GET {this}.Get called.");
            var objectsFromDb = _unitOfWork.ProductRepository
                .GetPagedList(pagingParameters, _productType.ToString());
            var metadata = PagedList<Product>.GetMetadata(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            _logger.LogInformation($"GET {this}.Get finished.");
            return Ok(objectsFromDb);
        }

        /// <summary>
        /// Gets product.
        /// </summary>
        /// <response code="200">Returns product from db</response>
        /// <response code="400">If the id parameter is not int</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the product from db is null</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"GET({id}) {this}.Get called.");
            var result = await _productService.Get(id); 
            _logger.LogInformation($"GET({id}) {this}.Get finished.");
            return StatusCode(result.Status, result.Data);
        }

        /// <summary>
        /// Creates product.
        /// </summary>
        /// <response code="201">If the product successfully created</response>
        /// <response code="400">If the photo is not an image or model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="500">If unknown error occurred while creating</response>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] EditProductDTO objectFromPage, IFormFile photo)
        {
            _logger.LogInformation($"POST {this}.Create called.");
            if (photo != null && !photo.ContentType.Contains("image"))
            {
                _logger.LogWarning("Parameter photo is not an image");
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            }
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(objectFromPage);
                product.ProductType = _productType.ToString();
                var result = await _productService.Create(product, photo);
                _logger.LogInformation($"POST {this}.Create finished.");
                return StatusCode(result.Status, result.Data);
            }
            else
            {
                _logger.LogWarning("ModelState is not valid");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates product.
        /// </summary>
        /// <response code="201">If the product successfully updated</response>        
        /// <response code="400">If the photo is not an image or model is not valid</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the product from db is null</response>
        /// <response code="500">If unknown error occurred while updating</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromForm] EditProductDTO objectFromPage, IFormFile photo)
        {
            _logger.LogInformation($"PUT {this}.Update called.");
            if (photo != null && !photo.ContentType.Contains("image"))
            {
                _logger.LogWarning("Parameter photo is not an image");
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            }
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(objectFromPage);
                product.Id = id;
                var result = await _productService.Update(product, photo);
                _logger.LogInformation($"PUT {this}.Update finished.");
                return StatusCode(result.Status, result.Data);
            }
            else
            {
                _logger.LogWarning("ModelState is not valid");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes product.
        /// </summary>
        /// <response code="200">If the product successfully deleted</response>
        /// <response code="400">If the id parameter is not int</response>
        /// <response code="401">Unathorized</response>
        /// <response code="404">If the product from db is null</response>
        /// <response code="409">If the product is already deleted</response>
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
            var result = await _productService.Delete(id);
            _logger.LogInformation($"DELETE {this}.DELETE finished.");
            return StatusCode(result.Status, result.Data);
        }
    }
}
