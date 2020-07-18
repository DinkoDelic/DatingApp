using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;
        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            //New cloudinary account using out settings
            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            //Checks the user id that we can from auth token
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            //Photo file from DTO
            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();
            //Check if the file is not empty
            if (file.Length > 0)
            {
                //Using to close the file stream after it's done
                using (var stream = file.OpenReadStream())
                {
                    //Uploading image with specified params
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        //Crops large images to 500x500px with a focus on face (functionality provied with Cloudinary)
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };


                    //Store result of upload
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            //Populating our DTO with properties from uploadResults
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            //Populates out Photo model with DTO properties
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            //Checks if user has any photos, if not set the first uploaded on to main profile picture
            if (!userFromRepo.Photos.Any(p => p.Main))
                photo.Main = true;
            //Adds photo to user photos
            userFromRepo.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
                //Populates our PhotoToReturnDTO with Photo properties
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not upload a photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {

            //Checks the user id that we get from auth token
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            //Check to see if the photo matches users photo id
            if (!userFromRepo.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.Main == true)
                return BadRequest("Photo is already main photo");

            var mainPhoto = await _repo.GetMainPhoto(userId);
            mainPhoto.Main = false;
            photoFromRepo.Main = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            //Checks the user id that we get from auth token
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            //Check to see if the photo matches users photo id
            if (!userFromRepo.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.Main == true)
                return BadRequest("Cannot delete main photo");

            if (photoFromRepo.PublicId != null)
            {
                //Deletion paramater for cloudianry (our photo public id)
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);
                //Return a string of text "ok" on deletion
                if (result.Result == "ok")
                    _repo.Delete(photoFromRepo);


                if (await _repo.SaveAll())
                    return Ok();

                return BadRequest("Could not delete photo.");
            }
            else
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Could not delete photo.");

        }
    }
}