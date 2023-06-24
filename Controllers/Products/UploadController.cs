using core7_oracle_angular14.Models;
using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_oracle_angular14.Controllers.Products
{
    [ApiExplorerSettings(GroupName = "Upload Product Picture")]
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase {

        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<UploadController> _logger;

        public UploadController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<UploadController> logger
            )
        {
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;        
        }  

        [HttpPost("/api/uploadproductpicture")]
        public IActionResult uploadPicture([FromForm]ProductUploadModel model) {
                if (model.Prod_pic.FileName != null)
                {
                    try
                    {
                        string ext= Path.GetExtension(model.Prod_pic.FileName);
                        var folderName = Path.Combine("Resources", "users/");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        var newFilename =pathToSave + "00" + model.Prodid + ".jpg";
                        using var image = SixLabors.ImageSharp.Image.Load(model.Prod_pic.OpenReadStream());
                        image.Mutate(x => x.Resize(100, 100));
                        image.Save(newFilename);

                        if (model.Prod_pic != null)
                        {
                            string file = "http://localhost:5099/resources/products/00"+model.Prodid.ToString()+".jpg";
                            _productService.UpdatePicture(model.Prodid, file);                            
                        }
                        return Ok(new { statuscode = 200, message = "Product Picture has been updated."});
                    }
                    catch (Exception ex)
                    {
                        return Ok(new {statuscode = 200, message =ex.Message});
                    }
                }
                return Ok(new { statuscode = 404, message = "Product Picture not found."});
        }        
    }
}