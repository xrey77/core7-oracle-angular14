using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Helpers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace core7_oracle_angular14.Controllers.Auth
{
    
[ApiExplorerSettings(GroupName = "Sign-in to User Account")]
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private IAuthService _authService;
    private readonly IConfiguration _configuration;  

    private readonly IWebHostEnvironment _env;

    private readonly ILogger<LoginController> _logger;

    public LoginController(
        IConfiguration configuration,
        IWebHostEnvironment env,
        IAuthService authService,
        ILogger<LoginController> logger
        )
    {
        _configuration = configuration;  
        _authService = authService;
        _logger = logger;
        _env = env;        
    }  

    [HttpPost("/signin")]
    public IActionResult signin([FromBody]UserLogin model) {
            try {
                 var xuser = _authService.SignUser(model.Username);
                 if (xuser.Username is not null) {
                    if (!BCrypt.Net.BCrypt.Verify(model.Pasword, xuser.Pasword)) {
                        return Ok(new {statuscode = 404, message="Incorrect Password..." });
                    }
                    if (xuser.Isactivated == 0) {
                        return Ok(new { StatusCode=404, message="Please activate your account, check your email inbox."});
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var xkey = _configuration["AppSettings:Secret"];
                    var key = Encoding.ASCII.GetBytes(xkey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, xuser.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    return Ok(new { 
                        statuscode = 200,
                        message = "Login Successfull, please wait..",
                        id = xuser.Id,
                        lastname = xuser.Lastname,
                        firstname = xuser.Firstname,
                        username = xuser.Username,
                        roles = xuser.Roles,
                        isactivated = xuser.Isactivated,
                        isblocked = xuser.Isblocked,
                        profilepic = xuser.Profilepic,
                        qrcodeurl = xuser.Qrcodeurl,
                        token = tokenString
                        });
                 } else {
                    return Ok(new { statuscode = 404, message = "Username not found.."});
                 }
            }
            catch (Exception ex)
            {
                return Ok(new {Message = ex.Message});
            }
    }
}
    
}