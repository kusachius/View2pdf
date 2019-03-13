using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kusachius.html2pdf
{
    public class Html2PdfService : IHtml2PdfService
    {
        /// <summary>
        /// Html2s the pdf async.
        /// </summary>
        /// <returns>The pdf async.</returns>
        /// <param name="html">Html.</param>
        public async Task<byte[]> Html2PdfAsync(string html)
        {
            var client = new HttpClient();

            ByteArrayContent bytes = new ByteArrayContent(System.Text.Encoding.Unicode.GetBytes(html));

            var x = new StringContent(html);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            //multiContent.Add(bytes, "file", "xxx.html");
            multiContent.Add(x, "file", "xxx.html");

            var response = client.PostAsync("http://127.0.0.1:32768", multiContent).Result;
            byte[] pdf = await response.Content.ReadAsByteArrayAsync();

            return pdf;
        }

        /// <summary>
        /// Htmls the file2 pdf file async.
        /// </summary>
        /// <returns>The file2 pdf file async.</returns>
        /// <param name="htmlFile">Html file.</param>
        public async Task<string> HtmlFile2PdfFileAsync(string htmlFile)
        {
            throw new NotImplementedException();
        }
    }
}
