using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CocoriCore.PageLogs
{
    public class PageMiddleware
    {
        public async Task InvokeAsync(HttpContext ctx, Func<Task> next)
        {
            if (ctx.Request.Path.Value.StartsWith("/api"))
                await next();
            else
            {
                if (ctx.Request.Path == "/tests")
                {
                    ctx.Response.ContentType = "text/html";
                    //await ctx.Response.WriteAsync(File.ReadAllText("Comptes.Api/tests.html"));
                }
                else
                {
                    ctx.Response.ContentType = "text/html";
                    //await ctx.Response.WriteAsync(File.ReadAllText("Comptes.Api/page2.html"));
                }
            }
        }
    }
}