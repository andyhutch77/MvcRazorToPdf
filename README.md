#MvcRazorToPdf
Create pdf documents within an asp .net mvc project by generating your views as normal but returning a PdfActionResult.

*Demo link to follow

This converts regular produced razor/html to pdf documents in the browser using the iTextXmlWorker.

This uses the newer version/licence of iText/iTextXmlWorker and can process the html that is produced from your views.

##Installation instructions:

Run the following nuget command from your mvc project to install the [package] (http://nuget.org/packages/MvcRazorToPdf/):

`Install-Package MvcRazorToPdf`

##How to use it
[See the MvcRazorToPdfExample project] (https://github.com/andyhutch77/MvcRazorToPdf/tree/master/MvcRazorToPdfExample)

Create a layout template that will work as detailed in the [iTextXmlWorker docs] (http://demo.itextsupport.com/xmlworker/itextdoc/flatsite.html).
This is also useful [http://demo.itextsupport.com/xmlworker/] (http://demo.itextsupport.com/xmlworker/) if you have problems getting certain styles to render.
Here is a one that I used in the demo []https://github.com/andyhutch77/MvcRazorToPdf/blob/master/MvcRazorToPdfExample/Views/Shared/_PdfLayout.cshtml(https://github.com/andyhutch77/MvcRazorToPdf/blob/master/MvcRazorToPdfExample/Views/Shared/_PdfLayout.cshtml).
It's pretty basic as the iTextXmlWorker needs to process it but the html is only ever rendered within a pdf document.


