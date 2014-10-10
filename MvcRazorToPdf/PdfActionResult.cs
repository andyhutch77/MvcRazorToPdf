using System.IO;
using System.Text;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

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

        public string ViewName { get; set; }
        public object Model { get; set; }

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
                viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
                viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                    context.Controller.TempData, context.HttpContext.Response.Output);
                viewEngineResult.Render(viewContext, context.HttpContext.Response.Output);
            }
            else
            {
                using (var document = new Document())
                {
                    using (var workStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                        writer.CloseStream = false;

                        document.Open();

                        viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
                        var sb = new StringBuilder();

                        using (TextWriter tr = new StringWriter(sb))
                        {
                            viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                                context.Controller.TempData, tr);
                            viewEngineResult.Render(viewContext, tr);
                            using (var reader = new StringReader(sb.ToString()))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);

                                document.Close();
                                new FileContentResult(workStream.ToArray(), "application/pdf").ExecuteResult(context);
                            }
                        }
                    }
                }
            }
        }
    }
}