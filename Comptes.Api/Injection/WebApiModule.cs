using System;
using CocoriCore;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;
using Comptes;
using Newtonsoft.Json;
using Ninject.Extensions.ContextPreservation;
using Ninject;
using Jose;
using CocoriCore.Router;
using CocoriCore.Page;
using CocoriCore.PageLogs;
using Newtonsoft.Json.Converters;

namespace Comptes.Api
{
    public class WebApiModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IHashService>().To<HashService>().InSingletonScope();
            this.Bind<IClock>().To<Clock>().InSingletonScope();

            // Repository
            this.Bind<IUIDProvider>().To<UIDProvider>().InSingletonScope();
            this.Bind<IInMemoryEntityStore>().To<InMemoryEntityStore>().InSingletonScope();
            this.Bind<IRepository>().To<MemoryRepository>().InNamedScope("unitofwork");


            this.Bind<HandlerFinder>().ToConstant(
                new HandlerFinder(
                    CocoriCore.Page.AssemblyInfo.Assembly,
                    CocoriCore.PageLogs.AssemblyInfo.Assembly,
                    Comptes.AssemblyInfo.Assembly,
                    this.GetType().Assembly))
                .InSingletonScope();

            this.Bind<IMessageBus>().To<Comptes.MessageBus>().InNamedScope("unitofwork");
            this.Bind<IExecuteHandler>().To<Comptes.ExecuteHandler>().InNamedScope("unitofwork");
            this.Bind<PageMapperConfiguration>().ToConstant(
                new PageMapperConfiguration(
                    CocoriCore.PageLogs.AssemblyInfo.Assembly,
                    Comptes.AssemblyInfo.Assembly)
                );
            this.Bind<IPageMapper>().To<PageMapper>().InSingletonScope();
            this.Bind<IClaimsProvider, IClaimsWriter>().To<ClaimsProviderAndWriter>().InNamedScope("unitofwork");

            // Middleware
            this.Bind<ApplicationMiddleware>().ToSelf();
            this.Bind<MessageDeserializer>().ToSelf();
            this.Bind<IErrorBus>().To<MyErrorBus>().InNamedScope("unitofwork");

            this.Bind<IHttpErrorWriter>().To<HttpErrorWriter>().InSingletonScope();
            this.Bind<HttpErrorWriterOptions>().ToConstant(HttpErrorWriterConfiguration.Options());
            //builder.RegisterAssemblyTypes(cocoriCoreAssembly, apiAssembly).AssignableTo<IHttpErrorWriterHandler>().AsSelf();

            this.Bind<IHttpResponseWriter>().To<HttpResponseWriter>().InSingletonScope();
            this.Bind<HttpResponseWriterOptions>().ToConstant(HttpResponseWriterConfiguration.Options());
            // builder.RegisterAssemblyTypes(cocoriCoreAssembly, cocoriCoreODataAssembly, apiAssembly).AssignableTo<IHttpReponseWriterHandler>().AsSelf();

            this.Bind<ITracer>().To<Tracer>();

            this.Bind<RouterOptions>().ToConstant(RouterConfiguration.Options());
            this.Bind<IRouter>().To<CocoriCore.Router.Router>().InSingletonScope();

            // Autres services
            var settings = new JsonSerializerSettings();
            this.Bind<JsonSerializer>().ToMethod(ctx =>
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(ctx.GetContextPreservingResolutionRoot().Get<PageConverter>());
                serializer.Converters.Add(ctx.GetContextPreservingResolutionRoot().Get<CallConverter>());
                serializer.Converters.Add(new StringEnumConverter());
                serializer.Converters.Add(new IDConverter());
                return serializer;
            }).InSingletonScope();
            this.Bind<IClock>().To<Clock>().InSingletonScope();

            this.Bind<PageLogsConfiguration>().ToConstant(new PageLogsConfiguration(
                Comptes.AssemblyInfo.Assembly
            ));
        }
    }
}
