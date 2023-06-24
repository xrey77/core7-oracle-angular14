using core7_oracle_angular14.Helpers;
using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace core7_oracle_angular14.Controllers.Users
{
    [ApiExplorerSettings(GroupName = "Delete User")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DeleteController : ControllerBase {

        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DeleteController> _logger;

        public DeleteController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<DeleteController> logger
            )
        {
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;        
        }  

        [HttpDelete("/api/deleteuser/{id}")]
        public IActionResult deleteUser(int id) {
            try {
               _userService.Delete(id);
                return Ok();
           } catch (AppException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}