using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class BasketItemViewModel
    {
        public int BasketId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public int AdvertId { get; set; }
        public int AdvertUserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime AdvertCreateDate { get; set; }
        public string AnimalName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string AnimalType { get; set; }
        public string ImagePath { get; set; }

        // Diğer Advert özellikleri

        
    }

}
