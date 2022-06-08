using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WazeCredit.Service.LifeTimeExample;

namespace WazeCredit.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context, TransientService transientService,
            ScopedService scopedService, SingletonService singletonService)
        {
            context.Items.Add("CustomMiddlewareTransient", "Transient MiddleWare - " + transientService.GetGuid());
            context.Items.Add("CustomMiddlewareScoped", "Scoped MiddleWare - " + scopedService.GetGuid());
            context.Items.Add("CustomMiddlewareSingleton", "Singleton MiddleWare - " + singletonService.GetGuid());

            await _next(context);
        }

    }
}
