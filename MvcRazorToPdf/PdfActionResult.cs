using System;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MvcRazorToPdf
{
    public class PdfActionResult : ActionResult
    {
        public PdfActionResult(string viewName, object model)
        {
            ViewName = viewName;
            Model = model;
        }

        public PdfActionResult(object model)
        {
            Model = model;
        }

        public PdfActionResult(object model, Action<PdfWriter, Document> configureSettings)
        {
            if (configureSettings == null) 
            throw new ArgumentNullException("configureSettings");
            Model = model;
            ConfigureSettings = configureSettings;
        }

        public PdfActionResult(string viewName, object model, Action<PdfWriter, Document> configureSettings)
        {
            if (configureSettings == null) 
            throw new ArgumentNullException("configureSettings");
            ViewName = viewName;
            Model = model;
            ConfigureSettings = configureSettings;
        }

        public string ViewName { get; set; }
        public object Model { get; set; }
        public Action<PdfWriter, Document> ConfigureSettings { get; set; }

        public string FileDownloadName { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            IView viewEngineResult;
            ViewContext viewContext;

            if (ViewName == null)
            {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = Model;


            if (context.HttpContext.Request.QueryString["html"] != null &&
                context.HttpContext.Request.QueryString["html"].ToLower().Equals("true"))
            {
                RenderHtmlOutput(context);
            }
            else
            {
                if (!String.IsNullOrEmpty(FileDownloadName))
                {
                    context.HttpContext.Response.AddHeader("content-disposition",
                        "attachment; filename=" + FileDownloadName);
                }

                new FileContentResult(context.GeneratePdf(Model, ViewName, ConfigureSettings), "application/pdf")
                    .ExecuteResult(context);
            }
        }

        private void RenderHtmlOutput(ControllerContext context)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
            var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                context.Controller.TempData, context.HttpContext.Response.Output);
            viewEngineResult.Render(viewContext, context.HttpContext.Response.Output);
        }
    }
}