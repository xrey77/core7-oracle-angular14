using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Services;
using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace core7_oracle_angular14.Controllers.Users
{
    [ApiExplorerSettings(GroupName = "List All Users")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GetallController : ControllerBase {
       

    private IUserService _userService;

    private readonly IConfiguration _configuration;  

    private readonly IWebHostEnvironment _env;

    private readonly ILogger<GetallController> _logger;

    public GetallController(
        IConfiguration configuration,
        IWebHostEnvironment env,
        IUserService userService,
        ILogger<GetallController> logger
        )
    {
        _configuration = configuration;  
        _userService = userService;
        _logger = logger;
        _env = env;        
    }  

        [HttpGet("/api/getallusers")]
        public IActionResult getAllusers() {
            try {
                
                var user = _userService.GetAll();
                return Ok(user);
            } catch(Exception ex) {
               return Ok(new {statuscode = 404, Message = ex.Message});
            }
        }
    }
    
}