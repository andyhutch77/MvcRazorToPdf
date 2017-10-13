using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;

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

                    //base64 image support : https://rupertmaier.wordpress.com/2014/08/22/creating-a-pdf-with-an-image-in-itextsharp/
                    var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                    tagProcessors.RemoveProcessor(HTML.Tag.IMG);
                    tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor());

                    var cssFiles = new CssFilesImpl();
                    cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                    var cssResolver = new StyleAttrCSSResolver(cssFiles);

                    var charset = Encoding.UTF8;
                    var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                    hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors);
                    var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(document, writer));
                    var cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                    var worker = new XMLWorker(cssPipeline, true);
                    var xmlParser = new XMLParser(true, worker, charset);

                    using (var reader = new StringReader(RenderRazorView(context, viewName)))
                    {
                        xmlParser.Parse(reader);
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