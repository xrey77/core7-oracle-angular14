using core7_oracle_angular14.Helpers;

namespace core7_oracle_angular14.Models
{
    public class ProductModel {
        public int Id { get; set; }
        public string Descriptions { get; set; }
        public int Qty { get; set; }
        public string Unit { get; set; }
        public decimal Cost_price { get; set; }
        public decimal Sell_price { get; set; }
        public string Prod_pic { get; set; }
        public string Category { get; set; }
        public decimal Sale_price { get; set; }
        public int Alert_level { get; set; }
        public int Critical_level { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }    
}