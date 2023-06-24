
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core7_oracle_angular14.Helpers;

namespace core7_oracle_angular14.Entities
{
    
[Table("products")]
public class Product {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        public string Descriptions { get; set; }

        [Column("qty")]
        public int Qty { get; set; }

        [Column("unit")]
        public string Unit { get; set; }

        [Column("cost_price",TypeName="decimal(10,2)")]
        public decimal Cost_price { get; set; }

        [Column("sell_price",TypeName="decimal(10,2)")]
        public decimal Sell_price { get; set; }

        [Column("prod_pic")]
        public string Prod_pic { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("sale_price",TypeName="decimal(10,2)")]
        public decimal Sale_price { get; set; }

        [Column("alert_level")]
        public int Alert_level { get; set; }

        [Column("critical_level")]
        public int Critical_level { get; set; }

        [Column("created_at")]
        public DateTime Created_at { get; set; }

        [Column("updated_at")]
        public DateTime Updated_at { get; set; }
    }    
}

