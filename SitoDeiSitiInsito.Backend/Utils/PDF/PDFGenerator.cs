using PdfSharp;
using PdfSharp.Pdf;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace SitoDeiSiti.Backend.Utils.PDF
{
    public class PDFGenerator
    {
        public string? TemplateBodyHtml { get; set; }
        public string? TemplateHeaderHtml { get; set; }
        public string? TemplateFooterHtml { get; set; }

        public PDFGenerator(string? templateBodyHtml)
        {
            TemplateBodyHtml = templateBodyHtml;
        }

        public PDFGenerator(string? templateBodyHtml, string? templateHeaderHtml, string? templateFooterHtml)
        {
            TemplateBodyHtml = templateBodyHtml;
            TemplateHeaderHtml = templateHeaderHtml;
            TemplateFooterHtml = templateFooterHtml;
        }


        public byte[]? GeneratePDF()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("<html><head>");
                sb.AppendLine(TemplateHeaderHtml ?? string.Empty);
                sb.AppendLine("</head><body>");
                sb.AppendLine(TemplateBodyHtml ?? string.Empty);
                sb.AppendLine("</body><footer>");
                sb.AppendLine(TemplateFooterHtml ?? string.Empty);
                sb.AppendLine("</footer></html>");

                Byte[] pdfBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    PdfDocument pdf = PdfGenerator.GeneratePdf(sb.ToString(), PageSize.A4);
                    pdf.Save(ms);
                    pdfBytes = ms.ToArray();
                }

                return pdfBytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
