using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using MvcRazorToPdf;
using MvcRazorToPdfExample.Models;
using Rectangle = iTextSharp.text.Rectangle;

namespace MvcRazorToPdfExample.Controllers
{
    public class PdfController : Controller
    {
        public ActionResult Index()
        {
            var model = new PdfExample {
                Heading = "Heading",
                Items = new List<BasketItem>(){
                    new BasketItem {
                     Id = 1,
                     Description = "Item 1",
                     Price = 1.99m},
                    new BasketItem {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m }
                }
            };
            return new PdfActionResult(model);
        }
        public ActionResult IndexWithAccessToDocumentAndWriter()
        {
            var model = new PdfExample
            {
                Heading = "Heading",
                Items = new List<BasketItem>(){
                    new BasketItem {
                     Id = 1,
                     Description = "Item 1",
                     Price = 1.99m},
                    new BasketItem {
                        Id = 2,
                        Description = "Item 2",
                        Price = 2.99m }
                }
            };
            return new PdfActionResult(model, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(500f, 500f, 90));
                document.NewPage();

            });
        }
    }
}
