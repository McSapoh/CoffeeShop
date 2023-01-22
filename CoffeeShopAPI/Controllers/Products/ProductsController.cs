using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.Products;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers.Products
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductsController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IProductService _productService;
        protected readonly IMapper _mapper;
        protected readonly ProductType _productType;

        public ProductsController(IUnitOfWork unitOfWork,
            IProductService productService, IMapper mapper, ProductType productType)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _mapper = mapper;
            _productType = productType;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.ProductRepository
                .GetPagedList(pagingParameters, _productType.ToString());
            var metadata = PagedList<Product>.GetMetadata(objectsFromDb);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(objectsFromDb);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _productService.Get(id);
            return StatusCode(result.Status, result.Data);
        }
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] EditProductDTO objectFromPage, IFormFile photo)
        {
            if (photo != null && !photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(objectFromPage);
                product.ProductType = _productType.ToString();
                var result = await _productService.Create(product, photo);
                return StatusCode(result.Status, result.Data);
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromForm] EditProductDTO objectFromPage, IFormFile photo)
        {
            if (photo != null && !photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            if (ModelState.IsValid)
            {
                if (id == 0)
                    return BadRequest(new JsonResult(new { success = false, message = $"Cannot find object with id = {id}" }));

                var product = _mapper.Map<Product>(objectFromPage);
                product.Id = id;
                var result = await _productService.Update(product, photo);
                return StatusCode(result.Status, result.Data);
            }
            else
                return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            return StatusCode(result.Status, result.Data);
        }
    }
}
