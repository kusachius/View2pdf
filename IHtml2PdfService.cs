using System;
using System.Threading.Tasks;

namespace Kusachius.html2pdf
{
    public interface IHtml2PdfService
    {
        Task<byte[]> Html2PdfAsync(string html);
        Task<string> HtmlFile2PdfFileAsync(string htmlFile);
    }
}
