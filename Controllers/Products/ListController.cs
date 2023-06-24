using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Services;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Helpers;

namespace core7_oracle_angular14.Controllers.Products
{
    [ApiExplorerSettings(GroupName = "List all Products")]
    [ApiController]
    [Route("[controller]")]
    public class ListController : ControllerBase {

        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ListController> _logger;

        public ListController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<ListController> logger
            )
        {
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;        
        }  

        [HttpGet("/listproducts/{page}")]
        public IActionResult ListProducts(int page) {
            try {                
                int totalpage = _productService.TotPage();
                var products = _productService.ListAll(page);
                if (products != null) {
                    return Ok(new {totpage = totalpage, page = page, products=products});
                } else {
                    return Ok(new {statuscode=404, message="No Data found."});
                }
            } catch(AppException ex) {
               return Ok(new {statuscode = 404, Message = ex.Message});
            }
        }
    }    
}