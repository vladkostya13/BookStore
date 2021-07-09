using AutoMapper;
using BookStore.API.Dtos.User;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            var isRolesExists = userRegistrationDto.Roles.All(x => _roleManager.RoleExistsAsync(x).Result);
            if (isRolesExists)
                return NotFound("Roles not founded");

            var user = _mapper.Map<User>(userRegistrationDto);

            var registrationResult = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (!registrationResult.Succeeded)
            {
                foreach (var error in registrationResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userRegistrationDto.Roles);

            return StatusCode(201);
        }
    }
}