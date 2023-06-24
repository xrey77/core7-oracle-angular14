using core7_oracle_angular14.Models;
using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Helpers;

namespace core7_oracle_angular14.Controllers.Auth
{
[ApiExplorerSettings(GroupName = "Sign-up or Account Registration")]
[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private IAuthService _authService;

    private readonly IWebHostEnvironment _env;

    private readonly ILogger<RegisterController> _logger;

    public RegisterController(
        IWebHostEnvironment env,
        IAuthService userService,
        ILogger<RegisterController> logger
        )
    {   
        _authService = userService;
        _logger = logger;
        _env = env;
    }  

    [HttpPost("/signup")]
    public IActionResult signup(UserRegister model) {
            try
            {
                _authService.SignupUser(model);
                return Ok(new {statuscode = 200, message = "You have registered successfully."});
            }
            catch (AppException ex)
            {
                return Ok(new { statuscode = 404, message = ex.Message });
            }
    }
}
    
}