using Ninject.Extensions.ContextPreservation;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;

namespace CocoriCore.Page
{
    public class CocoricorePageNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPageMapper>().To<PageMapper>().InSingletonScope();
            Bind<IExecuteHandler>().To<ExecuteHandler>().InNamedScope("unitofwork");
            Bind<IEmailReader, IEmailSender>().To<TestEmailSenderAndReader>().InSingletonScope();

            // claims
            Bind<TestBrowserClaimsProvider>().ToConstant(new TestBrowserClaimsProvider(response =>
            {
                if (response is IClaimsResponse claimsResponse)
                    return claimsResponse.GetClaims();
                return null;
            }));
            Bind<IClaimsProvider, IClaimsWriter>().To<ClaimsProviderAndWriter>().InNamedScope("unitofwork");
            Bind<UserFluentFactory>().ToSelf().DefinesNamedScope("user");
            Bind<UserFluent>().ToSelf().InNamedScope("user");
            Bind<ICurrentUserLogger>().To<CurrentUserLogger>().InNamedScope("user");
            Bind<IBrowser>().To<TestBrowser>().InNamedScope("user");
            Bind<IUserLogger, UserLogger>().To<UserLogger>().InSingletonScope();
        }
    }
}
