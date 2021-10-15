using System;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MvcRazorToPdf
{
    /// <summary>
    /// Mvc Razor Pdf Extensions
    /// </summary>
    public static class MvcRazorToPdfExtensions
    {
        /// <summary>
        /// GeneratePdf
        /// </summary>
        /// <param name="context">Controller contetxt</param>
        /// <param name="model">Model</param>
        /// <param name="viewName">ViewName</param>
        /// <param name="configureSettings">configuration settings</param>
        /// <returns></returns>
        public static byte[] GeneratePdf(this ControllerContext context, object model=null, string viewName=null,
            Action<PdfWriter, Document> configureSettings=null)
        {
            return new MvcRazorToPdf().GeneratePdfOutput(context, model, viewName, configureSettings);
        }
    }
}