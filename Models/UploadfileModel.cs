using Microsoft.AspNetCore.Mvc;

namespace core7_oracle_angular14.Models
{
    public class UploadfileModel {
        public int Id { get; set; }
        public IFormFile Profilepic { get; set; }

    }
    
}