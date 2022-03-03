using Domain.Entities;
using Domain.Interfaces.Identity;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.LoginViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        // private readonly IMailSender _mailSender;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IIdentityService identityService, IConfiguration configuration, UserManager<User> userManager, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _identityService = identityService;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] LoginRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            //var user = await _userService.GetUserAsync(model.Email);
            if (user != null)
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, $"{model.Password}{user.HashSalt}");
                if (isValidPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = _identityService.GenerateToken(user, roles);
                    var tokenResponse = new LoginResponseModel
                    {
                        Message = "Login Successful",
                        Status = true,
                        Data = new LoginResponseData
                        {
                            Roles = roles,
                            Email = user.Email,
                            LastName = user.LastName,
                            FirstName = user.FirstName,
                            UserId = user.Id
                        }
                    };
                    var expiry = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetValue<string>("JwtTokenSettings:TokenExpiryPeriod")));
                    Response.Headers.Add("Token", token);
                    Response.Headers.Add("TokenExpiry", expiry.ToUnixTimeMilliseconds().ToString());
                    Response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                    return Ok(tokenResponse);
                }
            }
            var response = new BaseResponse
            {
                Message = "Invalid Credential",
                Status = false
            };
            return BadRequest(response);
        }
    }
}
