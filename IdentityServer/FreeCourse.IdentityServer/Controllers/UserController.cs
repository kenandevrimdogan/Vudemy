﻿using FreeCourse.IdentityServer.Dtos.Users;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
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

        [HttpGet]
        public async Task<ActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            if (user == null)
            {
                return BadRequest();
            }


            return Ok(new { user.Id, user.UserName, user.Email, user.City });
        }
    }
}
