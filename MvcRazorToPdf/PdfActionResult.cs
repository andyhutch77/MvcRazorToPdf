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
        public string ViewName { get; set; }
        public object Model { get; set; }

        public PdfActionResult(string viewName, object model)
        {
            ViewName = viewName;
            Model = model;
        }

        public PdfActionResult(object model)
        {
            Model = model;
        }

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
                var workStream = new MemoryStream();
                var document = new Document();

                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;

                document.Open();

                viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
                var sb = new StringBuilder();
                TextWriter tr = new StringWriter(sb);

                viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                    context.Controller.TempData, tr);
                viewEngineResult.Render(viewContext, tr);
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stream, null);

                document.Close();

                new FileContentResult(workStream.ToArray(), "application/pdf").ExecuteResult(context);
            }
        }
    }
}
