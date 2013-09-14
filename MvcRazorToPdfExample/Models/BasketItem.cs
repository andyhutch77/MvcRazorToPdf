using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcRazorToPdfExample.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}