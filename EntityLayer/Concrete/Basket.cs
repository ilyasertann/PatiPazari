using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Basket
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int AdvertId { get; set; }

        // Relationship with Advert
    }
}
