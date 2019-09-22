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
                    Comptes.AssemblyInfo.Assembly,
                    this.GetType().Assembly))
                .InSingletonScope();

            this.Bind<IMessageBus>().To<Comptes.MessageBus>().InNamedScope("unitofwork");
            this.Bind<IExecuteHandler>().To<Comptes.ExecuteHandler>().InNamedScope("unitofwork");
            this.Bind<IPageMapper>().ToConstant(new PageMapper(Comptes.AssemblyInfo.Assembly));
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
                return serializer;
            }).InSingletonScope();
            this.Bind<IClock>().To<Clock>().InSingletonScope();

            /*
            this.Bind<JWTConfiguration>().ToConstant(new JWTConfiguration("monsecret", JwsAlgorithm.ES256));
            this.Bind<ITokenService>().To<TokenService>().InSingletonScope();
            this.Bind<IHttpAuthenticator, IClaimsProvider>().To<JwtAuthenticator>().InNamedScope("unitofwork");
            */
        }
    }
}
