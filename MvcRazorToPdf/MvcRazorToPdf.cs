using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace MvcRazorToPdf
{
    public class MvcRazorToPdf
    {
        public byte[] GeneratePdfOutput(ControllerContext context, object model = null, string viewName = null,
            Action<PdfWriter, Document> configureSettings = null)
        {
            if (viewName == null)
            {
                viewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = model;

            byte[] output;
            using (var document = new Document())
            {
                using (var workStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                    writer.CloseStream = false;

                    if (configureSettings != null)
                    {
                        configureSettings(writer, document);
                    }
                    document.Open();


                    using (var reader = new StringReader(RenderRazorView(context, viewName)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);

                        document.Close();
                        output = workStream.ToArray();
                    }
                }
            }
            return output;
        }

        public string RenderRazorView(ControllerContext context, string viewName)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, viewName, null).View;
            var sb = new StringBuilder();


            using (TextWriter tr = new StringWriter(sb))
            {
                var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                    context.Controller.TempData, tr);
                viewEngineResult.Render(viewContext, tr);
            }
            return sb.ToString();
        }
    }
}