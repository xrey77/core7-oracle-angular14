using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Authorization;

namespace core7_oracle_angular14.Controllers.Users
{
        [ApiExplorerSettings(GroupName = "Upload User Profice Picture")]
        [ApiController]
        [Route("[controller]")]    
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        [Authorize]
        public class UploadpictureController : ControllerBase {

        private IUserService _userService;
        private readonly IConfiguration _configuration;  

        private readonly IWebHostEnvironment _env;

        private readonly ILogger<UploadpictureController> _logger;

        public UploadpictureController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<UploadpictureController> logger
            )
        {
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;        
        }  
        
        [HttpPost("/api/uploadpicture")]
        public IActionResult uploadPicture([FromForm]UploadfileModel model) {
                if (model.Profilepic.FileName != null)
                {
                    try
                    {
                        string ext= Path.GetExtension(model.Profilepic.FileName);

                        var folderName = Path.Combine("Resources", "users/");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        var newFilename =pathToSave + "00" + model.Id + ".jpg";

                        using var image = SixLabors.ImageSharp.Image.Load(model.Profilepic.OpenReadStream());
                        image.Mutate(x => x.Resize(100, 100));
                        image.Save(newFilename);

                        if (model.Profilepic != null)
                        {
                            string file = "http://localhost:5099/resources/users/00"+model.Id.ToString()+".jpg";
                            _userService.UpdatePicture(model.Id, file);                            
                        }
                        return Ok(new { statuscode = 200, message = "Profile Picture has been updated."});



                        // using (var stream = new FileStream(
                        //     newFilename, FileMode.Create, FileAccess.Write, FileShare.Write, 1024))
                        // {
                        //     stream.Write(imageBytes, 0, imageBytes.Length);
                        // }

                        //     var file = Request.Form.Files[0];
                        //     var folderName = Path.Combine("Resources", "users");
                        //     var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        //     if (file.Length > 0)
                        //     {
                        //         var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //         var newFilename = "00" + id + ".jpeg";
                        //         var fullPath = Path.Combine(pathToSave, newFilename);
                        //         var dbPath = Path.Combine(folderName, fileName);

                        //         using (var stream = new FileStream(fullPath, FileMode.Create))
                        //         {
                        //             file.CopyTo(stream);
                        //         }
                        //         return Ok(new { dbPath });
                        //     }
                        //     else
                        //     {
                        //         return BadRequest();
                        //     }


                        
                    }
                    catch (Exception ex)
                    {
                        return Ok(new {statuscode = 200, message =ex.Message});
                    }

                }
                return Ok(new { statuscode = 404, message = "Profile Picture not found."});

        }
    }
    
}