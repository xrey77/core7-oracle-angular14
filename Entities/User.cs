using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core7_oracle_angular14.Entities
{
    [Table("users")]
    public class User {

        [Key]
        [Column("id")]
        public int Id {get; set;}

        [Column("lastname")]
        public string Lastname {get; set;}

        [Column("firstname")]
        public string Firstname {get; set;}

        [Column("username")]
        public string Username {get; set;}

        [Column("pasword")]
        public string Pasword {get; set;}

        [Column("email")]
        public string Email { get; set; }

        [Column("mobile")]
        public string Mobile { get; set; }

        [Column("roles")]        
        public string Roles { get; set; }

        [Column("isactivated")]
        public int Isactivated {get; set;}

        [Column("isblocked")]
        public int Isblocked {get; set;}

        [Column("mailtoken")]
        public int Mailtoken {get; set;}

        [Column("qrcodeurl",TypeName = "ntext")]
        public string Qrcodeurl {get; set;}

        [Column("profilepic")]
        public string Profilepic {get; set;}

        [Column("secretkey")]
        public string Secretkey  {get; set;}

        [Column("date_created")]
        public DateTime Date_created  {get; set;}

        [Column("date_updated")]
        public DateTime Date_updated  {get; set;}

    }
}