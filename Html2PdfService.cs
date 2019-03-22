using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

            var pdfConvertUrl = Environment.GetEnvironmentVariable("PDF_CONVERT_URL");
            if (pdfConvertUrl == null)
            {
                pdfConvertUrl = "http://127.0.0.1:3333";
            }


            var response = client.PostAsync(pdfConvertUrl, multiContent).Result;
            byte[] pdf = await response.Content.ReadAsByteArrayAsync();

            return pdf;
        }

        public async Task<byte[]> Html2PdfAsync1(string html)
        {
            var client = new HttpClient();

            ByteArrayContent bytes = new ByteArrayContent(System.Text.Encoding.Unicode.GetBytes(html));

            var base64 = Convert.ToBase64String(await bytes.ReadAsByteArrayAsync());

            string json = JsonConvert.SerializeObject(new { contents = base64, options = new { margin_top = "6"  } });
            //string json = "{\"contents\":\"" + base64 + "\", \"options\" : {} }";


            var pdfConvertUrl = Environment.GetEnvironmentVariable("PDF_CONVERT_URL");
            if (pdfConvertUrl == null)
            {
                pdfConvertUrl = "http://127.0.0.1:3333";
                //pdfConvertUrl = "https://gooduser:secretpassword@docker-wkhtmltopdf-aas.herokuapp.com";
            }

            var cont = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(pdfConvertUrl, cont ).Result;
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
