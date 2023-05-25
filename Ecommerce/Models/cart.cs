using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class cart
    {

/*        public Product Product { get; set; }
        public int Quantity { get; set; }

        public cart(Product product, int Qty)
        {
            Product = product;
            Quantity = Qty;


        }*/








        public int id { get; set; }


        public string Image { get; set; }
        public string Name { get; set; }
        public Int32 Qty { get; set; }

        public Int32 Price { get; set; }

        public int Total { get; set; }
        /*  public Product pd { get; set; }

          */
        public virtual Order Order { get; set; }

    }


}

