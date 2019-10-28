using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Location.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Location.Filters
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHubContext<Informacije> _info;

        public RequestMiddleware(RequestDelegate next, IHubContext<Informacije> info)
        {
            _next = next;
            _info = info;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var model = new InfoModel { Time = DateTime.Now, Message = httpContext.Request.Path.ToString() };
            _info.Clients.All.SendAsync("ReciveInfo", JsonConvert.SerializeObject(model));
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
