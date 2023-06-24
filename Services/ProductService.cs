using System.Data;
using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Helpers;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace core7_oracle_angular14.Services
{
    public interface IProductService {
        IEnumerable<Product> ListAll(int page);
        IEnumerable<Product> SearchAll(string key);
        Boolean Add_Product(Product product);
        void UpdatePicture(int id, string file);

        int TotPage();
    }

    public class ProductService : IProductService
    {

        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;  

        public ProductService(
            IConfiguration configuration,
            IOptions<AppSettings> appSettings)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }        

        public int TotPage() {
          try {
            using(var _oracleCon2 = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd2 = _oracleCon2.CreateCommand())
            {
                _oracleCon2.Open();
                cmd2.CommandText = @"SELECT COUNT(*) as cnt FROM products";
                OracleDataReader recordsReader = cmd2.ExecuteReader();
                recordsReader.Read();

                var perpage = 5;
                var totrecs = recordsReader.GetInt32(0);

                recordsReader.Dispose();
                _oracleCon2.Close();
                _oracleCon2.Dispose();
            
                int totpage = (int)Math.Ceiling((float)(totrecs) / perpage);
                return totpage;
            }
          } catch(Exception) {
            return 0;
          }
        }
        public IEnumerable<Product> ListAll(int page)
        {
            var perpage = 5;
            var offset = (page -1) * perpage;

            List<Product> listProducts = new List<Product>();
            try {
              using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
              using(var cmd = _oracleConnection.CreateCommand())
              {
                  _oracleConnection.Open();                
                  cmd.CommandText = @"SELECT prodid,descriptions,qty,unit,cost_price,sell_price,sale_price,alert_level,critical_level,category,prod_pic FROM products ORDER BY prodid ASC OFFSET '" + offset + "' ROWS FETCH NEXT '" + perpage + "' ROWS ONLY ";
                  OracleDataReader productReader = cmd.ExecuteReader();
                  Product prod;
                  while (productReader.Read()) {
                      prod  = new Product();
                      prod.Id = Convert.ToInt32(productReader["PRODID"]);
                      prod.Descriptions = Convert.ToString(productReader["DESCRIPTIONS"]);
                      prod.Qty = Convert.ToInt32(productReader["QTY"]);
                      prod.Unit = Convert.ToString(productReader["UNIT"]);
                      prod.Cost_price = Convert.ToDecimal(productReader["COST_PRICE"]);
                      prod.Sell_price = Convert.ToDecimal(productReader["SELL_PRICE"]);
                      prod.Sale_price = Convert.ToDecimal(productReader["SALE_PRICE"]);
                      prod.Prod_pic = Convert.ToString(productReader["PROD_PIC"]);
                      prod.Alert_level = Convert.ToInt32(productReader["ALERT_LEVEL"]);
                      prod.Critical_level = Convert.ToInt32(productReader["CRITICAL_LEVEL"]);
                      prod.Category = Convert.ToString(productReader["CATEGORY"]);
                      prod.Prod_pic =  Convert.ToString(productReader["PROD_PIC"]);
                      listProducts.Add(prod);
                  }
                  productReader.Close();
                  productReader.Dispose();
                  _oracleConnection.Close();
                  _oracleConnection.Dispose();
                  return listProducts;
              }
            } catch(Exception ex) {
              throw new AppException(ex.Message);
            }
        }

        public IEnumerable<Product> SearchAll(string key)
        {
            List<Product> listProducts = new List<Product>();
            try {
              using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
              using(var cmd = _oracleConnection.CreateCommand())
              {
                  _oracleConnection.Open();                
                  cmd.CommandText = @"SELECT prodid,descriptions,qty,unit,cost_price,sell_price,sale_price,alert_level,critical_level,category,prod_pic FROM products WHERE LOWER(descriptions) LIKE '%" + key.ToLower() + "%' ORDER BY prodid";
                  OracleDataReader productReader = cmd.ExecuteReader();
                  Product prod;
                  while (productReader.Read()) {
                      prod  = new Product();
                      prod.Id = Convert.ToInt32(productReader["PRODID"]);
                      prod.Descriptions = Convert.ToString(productReader["DESCRIPTIONS"]);
                      prod.Qty = Convert.ToInt32(productReader["QTY"]);
                      prod.Unit = Convert.ToString(productReader["UNIT"]);
                      prod.Cost_price = Convert.ToDecimal(productReader["COST_PRICE"]);
                      prod.Sell_price = Convert.ToDecimal(productReader["SELL_PRICE"]);
                      prod.Sale_price = Convert.ToDecimal(productReader["SALE_PRICE"]);
                      prod.Prod_pic = Convert.ToString(productReader["PROD_PIC"]);
                      prod.Alert_level = Convert.ToInt32(productReader["ALERT_LEVEL"]);
                      prod.Critical_level = Convert.ToInt32(productReader["CRITICAL_LEVEL"]);
                      prod.Category = Convert.ToString(productReader["CATEGORY"]);
                      prod.Prod_pic =  Convert.ToString(productReader["PROD_PIC"]);

                      listProducts.Add(prod);
                  }
                  productReader.Close();
                  productReader.Dispose();
                  _oracleConnection.Close();
                  _oracleConnection.Dispose();
                  return listProducts;
              }
            } catch(Exception ex) {
              throw new AppException(ex.Message);
            }
        }

        private bool findDescription(string _desc) {
            using(var _oracleCon1 = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd1 = _oracleCon1.CreateCommand())
            {
                _oracleCon1.Open();
                cmd1.CommandText = @"SELECT COUNT(*) FROM products WHERE DESCRIPTIONS = :DESCRIPTIONS";
                cmd1.Parameters.Add(new OracleParameter("DESCRIPTIONS", OracleDbType.Varchar2)).Value = _desc;
                OracleDataReader descReader = cmd1.ExecuteReader();
                cmd1.BindByName = true;                    
                descReader.Read();
                if (descReader.GetInt32(0) > 0) {
                    descReader.Dispose();
                    _oracleCon1.Close();
                    _oracleCon1.Dispose();
                    return true;
                } 
                descReader.Dispose();
                _oracleCon1.Close();
                _oracleCon1.Dispose();
            }    
            return false;
        }

        public bool Add_Product(Product product)
        {
            if (this.findDescription(product.Descriptions) is true)  {
              throw new Exception("Product Description is already taken.");
            }

            using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd = _oracleConnection.CreateCommand())
            {                            
                _oracleConnection.Open();
                cmd.CommandText = @"INSERT INTO products(PRODID,DESCRIPTIONS,QTY,UNIT,COST_PRICE,SELL_PRICE,SALE_PRICE,CATEGORY,ALERT_LEVEL,CRITICAL_LEVEL) VALUES(prodid.nextval, :DESCRIPTIONS, :QTY, :UNIT, :COST_PRICE, :SELL_PRICE, :SALE_PRICE, :CATEGORY, :ALERT_LEVEL, :CRITICAL_LEVEL)";
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("DESCRIPTIONS", OracleDbType.Varchar2)).Value = product.Descriptions;
                cmd.Parameters.Add(new OracleParameter("QTY", OracleDbType.Int32)).Value = product.Qty;
                cmd.Parameters.Add(new OracleParameter("UNIT", OracleDbType.Varchar2)).Value = product.Unit;
                cmd.Parameters.Add(new OracleParameter("COST_PRICE", OracleDbType.Decimal)).Value = product.Cost_price;
                cmd.Parameters.Add(new OracleParameter("SELL_PRICE", OracleDbType.Decimal)).Value = product.Sell_price;
                cmd.Parameters.Add(new OracleParameter("SALE_PRICE", OracleDbType.Decimal)).Value = product.Sale_price;
                cmd.Parameters.Add(new OracleParameter("CATEGORY", OracleDbType.Varchar2)).Value = product.Category;
                cmd.Parameters.Add(new OracleParameter("ALERT_LEVEL", OracleDbType.Int32)).Value = product.Alert_level;
                cmd.Parameters.Add(new OracleParameter("CRITICAL_LEVEL", OracleDbType.Int32)).Value = product.Critical_level;
                cmd.ExecuteNonQuery();
                _oracleConnection.Close();
                _oracleConnection.Dispose();
            }
            return true;
        }

        public void UpdatePicture(int id, string file)
        {
            try {
                using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
                using(var cmd = _oracleConnection.CreateCommand())
                {                            
                    _oracleConnection.Close();
                    _oracleConnection.Open();
                    cmd.CommandText = @"UPDATE products SET PROD_PIC = :PROD_PIC WHERE PRODID = :PRODID";
                    cmd.Parameters.Add(new OracleParameter("PROD_PIC", OracleDbType.Varchar2)).Value = file;
                    cmd.Parameters.Add(new OracleParameter("PRODID", OracleDbType.Int32)).Value = id;
                    cmd.ExecuteNonQuery();
                    _oracleConnection.Close();
                    _oracleConnection.Dispose();
                }
            } catch(Exception ex) {
                throw new AppException(ex.Message);
            }
        }
    }
}