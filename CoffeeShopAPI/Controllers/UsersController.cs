using AutoMapper;
using CoffeeShopAPI.Helpers.DTO.User;
using CoffeeShopAPI.Helpers.Paging;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Services;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDTO objectFromPage)
        {
            if (objectFromPage.Photo != null && !objectFromPage.Photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = "File is not a photo" }));
            if (ModelState.IsValid)
            {
                if (_unitOfWork.UserRepository.GetByEmail(objectFromPage.Email) != null)
                    return BadRequest(new JsonResult(new
                    {
                        success = false,
                        message = "User with this email is already exists"
                    }));
                var user = _mapper.Map<User>(objectFromPage);
                var imagePath = await _imagesService.SavePhoto("User", objectFromPage.Photo);
                if(imagePath != null)
                    user.ImagePath = imagePath;
                user.RegistrationDate = DateTime.Now;
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

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDTO objectFromPage)
        {
            if (objectFromPage.Photo != null && !objectFromPage.Photo.ContentType.Contains("image"))
                return BadRequest(new JsonResult(new { success = false, message = "File is not a photo" }));
            if (ModelState.IsValid)
            {
                var objectFromDb = _unitOfWork.UserRepository.GetById(objectFromPage.Id);
                if (objectFromDb == null)
                    return NotFound(new JsonResult(new
                    {
                        success = false,
                        message = $"Cannot find user with id {objectFromPage.Id}"
                    }));
                if (objectFromPage.Email != objectFromDb.Email)
                {
                    var checkEmailUser = _unitOfWork.UserRepository.GetByEmail(objectFromPage.Email);
                    if (checkEmailUser != null && checkEmailUser.Email == objectFromPage.Email)
                        return BadRequest(new JsonResult(new
                        {
                            success = false,
                            message = "User with this email is already exists"
                        }));
                }

                var user = _mapper.Map<User>(objectFromPage);
                var imagePath = await _imagesService.SavePhoto("User", objectFromPage.Photo);
                if (imagePath != null)
                    user.ImagePath = imagePath;
                objectFromDb.Adress = user.Adress;
                objectFromDb.Email = user.Email;
                objectFromDb.ImagePath = user.ImagePath;
                objectFromDb.Name = user.Name;
                objectFromDb.Password = user.Password;

                _unitOfWork.UserRepository.Update(objectFromDb);
                if (await _unitOfWork.SaveAsync())
                    return Ok(new JsonResult(new
                    {
                        success = true,
                        message = "Successfully updated"
                    }));
                else
                    return BadRequest(new JsonResult(new
                    {
                        success = false,
                        message = "Error while updating"
                    }));
            }
            else
                return BadRequest(ModelState);
        }

        [HttpGet("Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers([FromQuery] PagingParameters pagingParameters)
        {
            var objectsFromDb = _unitOfWork.UserRepository
                .GetPagedList(pagingParameters);
            var listObjectsFromDb = _mapper.Map<List<DisplayUserDTO>>(objectsFromDb);
            var convertedObjects = new PagedList<DisplayUserDTO>(listObjectsFromDb, objectsFromDb.Count,
                objectsFromDb.CurrentPage, objectsFromDb.PageSize);
            var metadata = PagedList<DisplayUserDTO>.GetMetadata(convertedObjects);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(convertedObjects);
        }
    }
}
