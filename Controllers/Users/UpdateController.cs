using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Helpers;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace core7_oracle_angular14.Controllers.Users
{
        [ApiExplorerSettings(GroupName = "Update User")]
        [ApiController]
        [Route("[controller]")]
        [Authorize]
        public class UpdateController : ControllerBase {
            
        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;

        private readonly ILogger<UpdateController> _logger;

        public UpdateController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<UpdateController> logger
            )
        {
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;        
        }  

        [HttpPost("/api/updateprofile/{id}")]
        public IActionResult updateUser(int id, [FromBody]UserUpdate model) {
            try
            {
                User user = new User();
                user.Id = id;
                user.Lastname = model.Lastname;
                user.Firstname = model.Firstname;
                user.Mobile = model.Mobile;
                user.Pasword = model.Pasword;
                _userService.Update(user);
                return Ok(new {statuscode = 200, message = "Your profile has been updated."});
            }
            catch (AppException ex)
            {
                return NotFound(new { statuscode = 404, message = ex.Message });
            }
        }
    }
}