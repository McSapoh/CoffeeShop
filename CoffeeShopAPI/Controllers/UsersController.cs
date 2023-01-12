using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagesService _imagesService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IImagesService imagesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagesService = imagesService;
        }
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDTO objectFromPage, IFormFile photo)
        {
            if (photo != null && !photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = $"File is not a photo" }));
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(objectFromPage);
                _unitOfWork.UserRepository.Create(user);
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new
                    {
                        success = true,
                        message = "Successfully saved"
                    }));
                else
                    return BadRequest(new JsonResult(new
                    {
                        success = false,
                        message = "Error while saving"
                    }));
            }
            else
                return BadRequest(ModelState);
        }
    }
}
