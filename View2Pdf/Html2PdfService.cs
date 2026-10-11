using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kusachius.html2pdf
{
	public class Html2PdfService : IHtml2PdfService
	{

		private async Task<HttpResponseMessage> Html2PdfResponseAsync(string html)
		{
			using (var client = new HttpClient())
			{

				using (ByteArrayContent bytes = new ByteArrayContent(System.Text.Encoding.Unicode.GetBytes(html)))
				{

					var x = new StringContent(html);
					MultipartFormDataContent multiContent = new MultipartFormDataContent
				{
					{ x, "file", "xxx.html" }
				};

					var pdfConvertUrl = Environment.GetEnvironmentVariable("PDF_CONVERT_URL");
					if (pdfConvertUrl == null)
					{
						pdfConvertUrl = "http://127.0.0.1:3333";
					}


					return await client.PostAsync(pdfConvertUrl, multiContent);
				}
			}
		}

		/// <summary>
		/// Html2s the pdf async.
		/// </summary>
		/// <returns>The pdf async.</returns>
		/// <param name="html">Html.</param>
		public async Task<byte[]> Html2PdfAsync(string html)
		{
			var response = await Html2PdfResponseAsync(html);
			return await response.Content.ReadAsByteArrayAsync();
		}

		/// <summary>
		/// Htmls the file2 pdf stream async.
		/// </summary>
		/// <returns>The file2 pdf stream async.</returns>
		/// <param name="html">Html.</param>
		public async Task<Stream> HtmlFile2PdfStreamAsync(string html)
		{
			var response = await Html2PdfResponseAsync(html);
			return await response.Content.ReadAsStreamAsync();
		}
	}
}
