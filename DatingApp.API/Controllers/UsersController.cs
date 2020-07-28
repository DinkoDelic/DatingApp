using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    //Only logged in user can access this info
    [Authorize]
    [Route("api/[controller]")]
    //http://localhost:5000/api/auth
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;
            // Sets gender parameter to opposite of logged in user gender 
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);
            //Maps users to provided destination object(Dto in this case) as a list
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages); 
            return Ok(usersToReturn);
        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, UserForEditDto userForEdit)
        {
            //Checks the user id that we can from auth token
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);
            //Updates the values in user model
            _mapper.Map(userForEdit, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();
            // $ in front is used for string interpolation
            throw new System.Exception($"Updating user {id} failed on save");
        }
    }
}