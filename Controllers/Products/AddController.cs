using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Models;
using core7_oracle_angular14.Services;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace core7_oracle_angular14.Controllers.Products
{
    [ApiExplorerSettings(GroupName = "Add New Product")]
    [ApiController]
    [Route("[controller]")]
     public class AddController : ControllerBase {

        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AddController> _logger;

        public AddController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<AddController> logger
            )
        {
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;        
        }  

        [HttpPost("/addproduct")]
        public IActionResult addProduct([FromBody]ProductModel model) {
            try {
                Product prod = new Product();
                prod.Descriptions = model.Descriptions;
                prod.Qty = model.Qty;
                prod.Unit = model.Unit;
                prod.Cost_price = model.Cost_price;
                prod.Sell_price = model.Sell_price;
                prod.Sale_price = model.Sale_price;
                prod.Category = model.Category;
                prod.Alert_level = model.Alert_level;
                prod.Critical_level = model.Critical_level;
                _productService.Add_Product(prod);
                return Ok(new {statuscode = 200, message = "New Product has been added."});                     
            } catch(Exception ex) {
                return BadRequest(new {statuscode = 500, ex.Message});
            }                         
        }

    }
    
}