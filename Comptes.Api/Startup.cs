using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Ninject;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Ninject;
using Ninject.Extensions.ContextPreservation;
using Ninject.Extensions.NamedScope;

namespace Comptes.Api
{
    public class Startup
    {
        //readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });*/
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var kernel = CreateKernel();
            var unitOfWorkFactory = kernel.Get<IUnitOfWorkFactory>();
            app.UseWebSockets();

            var currentDir = Directory.GetCurrentDirectory();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(currentDir, "Comptes.Api/wwwroot")),
                //RequestPath = ""
            });

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Path.Value.StartsWith("/api"))
                {
                    using (var unitOfWork = unitOfWorkFactory.NewUnitOfWork())
                    {
                        await unitOfWork.Resolve<ApplicationMiddleware>().InvokeAsync(ctx, next);
                    }
                }
                else if (ctx.Request.Path == "/ws")
                {
                    if (ctx.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await ctx.WebSockets.AcceptWebSocketAsync();
                        await Echo(ctx, webSocket);
                    }
                    else
                    {
                        ctx.Response.StatusCode = 400;
                    }
                }
                else if (ctx.Request.Path == "/tests")
                {
                    ctx.Response.ContentType = "text/html";
                    await ctx.Response.WriteAsync(File.ReadAllText("Comptes.Api/tests.html"));
                }
                else
                {
                    ctx.Response.ContentType = "text/html";
                    await ctx.Response.WriteAsync(File.ReadAllText("Comptes.Api/wwwroot/page.html"));
                }

                //await next();
            });


            // TODO restreindre CORS à l'environnement de développement
            // app.UseCors(MyAllowSpecificOrigins);

            // else
            // {
            //     app.UseExceptionHandler("/Error");
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     //app.UseHsts();
            // }

            // //app.UseHttpsRedirection();
            // //app.UseStaticFiles();
            // //app.UseCookiePolicy();
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }


        public static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(new NamedScopeModule());
            kernel.Load(new ContextPreservationModule());

            kernel.Load(new CocoricoreNinjectModule());
            kernel.Load(new WebApiModule());

            return kernel;
        }
    }
}