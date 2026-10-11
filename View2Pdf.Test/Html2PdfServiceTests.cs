using System.Net;
using System.Text;
using FluentAssertions;
using Kusachius.html2pdf;
using Xunit;

namespace Kusachius.View2Pdf.Test
{
	// Each test starts a local stub converter and points PDF_CONVERT_URL at it.
	[Collection("PdfConvertUrl")]
	public class Html2PdfServiceTests : IDisposable
	{
		private static readonly byte[] FakePdf = Encoding.ASCII.GetBytes("%PDF-1.4 fake");

		private readonly string? originalUrl = Environment.GetEnvironmentVariable("PDF_CONVERT_URL");
		private readonly HttpListener listener = new HttpListener();
		private readonly Task serverTask;
		private string? requestContentType;
		private string? requestBody;

		public Html2PdfServiceTests()
		{
			var port = GetFreePort();
			var url = $"http://127.0.0.1:{port}/";
			listener.Prefixes.Add(url);
			listener.Start();
			Environment.SetEnvironmentVariable("PDF_CONVERT_URL", url);
			serverTask = Task.Run(HandleOneRequestAsync);
		}

		public void Dispose()
		{
			listener.Close();
			Environment.SetEnvironmentVariable("PDF_CONVERT_URL", originalUrl);
		}

		private static int GetFreePort()
		{
			var tcp = new System.Net.Sockets.TcpListener(IPAddress.Loopback, 0);
			tcp.Start();
			var port = ((IPEndPoint)tcp.LocalEndpoint).Port;
			tcp.Stop();
			return port;
		}

		private async Task HandleOneRequestAsync()
		{
			var context = await listener.GetContextAsync();
			requestContentType = context.Request.ContentType;
			using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
			{
				requestBody = await reader.ReadToEndAsync();
			}

			context.Response.StatusCode = 200;
			context.Response.ContentType = "application/pdf";
			context.Response.ContentLength64 = FakePdf.Length;
			await context.Response.OutputStream.WriteAsync(FakePdf, 0, FakePdf.Length);
			context.Response.Close();
		}

		[Fact]
		public async Task Html2PdfAsync_ReturnsConverterResponseBytes()
		{
			var result = await new Html2PdfService().Html2PdfAsync("<h1>Hello</h1>");

			await serverTask;
			result.Should().Equal(FakePdf);
		}

		[Fact]
		public async Task Html2PdfAsync_SendsHtmlAsMultipartFile()
		{
			await new Html2PdfService().Html2PdfAsync("<h1>Hello</h1>");

			await serverTask;
			requestContentType.Should().StartWith("multipart/form-data");
			requestBody.Should().Contain("name=file");
			requestBody.Should().Contain("filename=xxx.html");
			requestBody.Should().Contain("<h1>Hello</h1>");
		}

		[Fact]
		public async Task HtmlFile2PdfStreamAsync_ReturnsConverterResponseStream()
		{
			using var stream = await new Html2PdfService().HtmlFile2PdfStreamAsync("<h1>Hello</h1>");
			using var buffer = new MemoryStream();
			await stream.CopyToAsync(buffer);

			await serverTask;
			buffer.ToArray().Should().Equal(FakePdf);
		}
	}
}
