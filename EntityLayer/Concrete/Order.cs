using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public int AdvertID { get; set; }
        public int UserID { get; set; }
        public string CardNameSurName { get; set; }
        public string CardCreditNumber { get; set; }
        public string CardCVV { get; set; }
        public DateTime CardDate { get; set; }

        public int BasketOrderID { get; set; }

      
      }

}

