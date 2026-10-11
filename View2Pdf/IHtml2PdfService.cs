using System.IO;
using System.Threading.Tasks;

namespace Kusachius.html2pdf
{
	public interface IHtml2PdfService
	{
		Task<byte[]> Html2PdfAsync(string html);
		Task<Stream> HtmlFile2PdfStreamAsync(string html);
	}
}
