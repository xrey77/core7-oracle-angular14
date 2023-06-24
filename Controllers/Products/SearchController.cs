using Microsoft.AspNetCore.Mvc;
using core7_oracle_angular14.Services;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Helpers;

namespace core7_oracle_angular14.Controllers.Products
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase {

        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<SearchController> _logger;

        public SearchController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<SearchController> logger
            )
        {
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;        
        }  

        [HttpPost("/searchproducts")]
        public IActionResult SearchProducts([FromBody]ProductSearch prod) {
            try {                
                var products = _productService.SearchAll(prod.Search);
                if (products.Count() > 0) {
                    return Ok(new {products=products});
                } else {
                    return Ok(new {statuscode=404, message="No Data found."});
                }
            } catch(AppException ex) {
               return Ok(new {statuscode = 404, Message = ex.Message});
            }
        }
    }    
}