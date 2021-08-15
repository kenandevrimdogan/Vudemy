﻿using FreeCourse.IdentityServer.Dtos.Users;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDTO signup)
        {
            var user = new ApplicationUser
            {
                UserName = signup.UserName,
                Email = signup.Email,
                City = signup.City
            };

            var result = await _userManager.CreateAsync(user, signup.Password);

            if (!result.Succeeded)
            {
                return BadRequest(ResponseDTO<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest));
            }

            return Ok(ResponseDTO<NoContent>.Success(HttpStatusCode.OK));
        }
    }
}
