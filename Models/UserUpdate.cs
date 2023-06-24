using System.ComponentModel.DataAnnotations;

namespace core7_oracle_angular14.Models
{
  public class UserUpdate
    {        
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }
        public string Pasword { get; set; }

        public string Mobile { get; set; }
        public IFormFile Profilepic { get; set; }
        public bool Twofactorenabled { get; set; }

      //  public DateTimeOffset Updatedat { get; set; }
    }

    
}