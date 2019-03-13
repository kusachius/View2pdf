using System;
using System.Threading.Tasks;

namespace Kusachius.View2Pdf
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
