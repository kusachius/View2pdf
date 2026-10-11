using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace Kusachius.View2Pdf.Test
{
	public class ViewRenderServiceTests
	{
		private readonly Mock<IRazorViewEngine> viewEngine = new Mock<IRazorViewEngine>();
		private readonly Mock<ITempDataProvider> tempDataProvider = new Mock<ITempDataProvider>();
		private readonly Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();

		private ViewRenderService CreateService() =>
			new ViewRenderService(viewEngine.Object, tempDataProvider.Object, serviceProvider.Object);

		[Fact]
		public async Task RenderToStringAsync_ThrowsWhenViewIsNotFound()
		{
			viewEngine
				.Setup(e => e.FindView(It.IsAny<ActionContext>(), "Missing", false))
				.Returns(ViewEngineResult.NotFound("Missing", new[] { "/Views/Missing.cshtml" }));

			Func<Task> act = () => CreateService().RenderToStringAsync("Missing", new object());

			var exception = await act.Should().ThrowAsync<ArgumentNullException>();
			exception.WithMessage("*Missing does not match any available view*");
		}

		[Fact]
		public async Task RenderToStringAsync_ReturnsRenderedViewOutput()
		{
			var view = new Mock<IView>();
			view.Setup(v => v.RenderAsync(It.IsAny<ViewContext>()))
				.Returns<ViewContext>(ctx => ctx.Writer.WriteAsync("<p>Hello</p>"));
			viewEngine
				.Setup(e => e.FindView(It.IsAny<ActionContext>(), "Invoice", false))
				.Returns(ViewEngineResult.Found("Invoice", view.Object));

			var html = await CreateService().RenderToStringAsync("Invoice", new object());

			html.Should().Be("<p>Hello</p>");
		}

		[Fact]
		public async Task RenderToStringAsync_PassesModelToView()
		{
			var model = new { Name = "Test" };
			object? receivedModel = null;
			var view = new Mock<IView>();
			view.Setup(v => v.RenderAsync(It.IsAny<ViewContext>()))
				.Returns<ViewContext>(ctx =>
				{
					receivedModel = ctx.ViewData.Model;
					return Task.CompletedTask;
				});
			viewEngine
				.Setup(e => e.FindView(It.IsAny<ActionContext>(), "Invoice", false))
				.Returns(ViewEngineResult.Found("Invoice", view.Object));

			await CreateService().RenderToStringAsync("Invoice", model);

			receivedModel.Should().BeSameAs(model);
		}
	}
}
