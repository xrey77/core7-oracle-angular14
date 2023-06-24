using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Services;
using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace core7_oracle_angular14.Controllers.Users
{
    [ApiExplorerSettings(GroupName = "Retrieve User by ID")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GetbyidController : ControllerBase
    {

    private IUserService _userService;

    private readonly IConfiguration _configuration;  

    private readonly IWebHostEnvironment _env;

    private readonly ILogger<GetbyidController> _logger;

    public GetbyidController(
        IConfiguration configuration,
        IWebHostEnvironment env,
        IUserService userService,
        ILogger<GetbyidController> logger
        )
    {
        _configuration = configuration;  
        _userService = userService;
        _logger = logger;
        _env = env;        
    }  

        [HttpGet("/api/getuserbyid/{id}")]
        public IActionResult getByid(int id) {
            try {
                var user = _userService.GetById(id);
                return Ok(new {
                    statuscode = 200,
                    message = "User found, please wait.",
                    user = user
                });

            } catch(AppException ex) {
                return NotFound(new {
                    statuscode = 404,
                    message = ex.Message
                });

            }
        }
    }
    
}