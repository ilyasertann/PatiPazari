using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Advert
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
         public int CategoryID { get; set; }
        public DateTime CreateDate { get; set; }
        public string AnimalName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string AnimalType { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
