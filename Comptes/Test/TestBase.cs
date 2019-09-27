using System;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Ninject;
using CocoriCore.Page;
using CocoriCore.Router;
using Ninject;
using Ninject.Extensions.ContextPreservation;
using Ninject.Extensions.NamedScope;
using Ninject.Extensions.Factory;
namespace Comptes
{
    public class TestBase : IPageTest// : IDisposable
    {
        private StandardKernel kernel;

        public TestBase()
        {
            kernel = new StandardKernel();
            kernel.Load(new NamedScopeModule());
            kernel.Load(new ContextPreservationModule());
            kernel.Load(new FuncModule());
            kernel.Load(new CocoricoreNinjectModule());
            kernel.Bind<IHashService>().To<HashService>().InSingletonScope();
            kernel.Bind<IClock, SettableClock>().To<SettableClock>().InSingletonScope();

            // Repository
            kernel.Bind<IUIDProvider>().To<UIDProvider>().InSingletonScope();
            kernel.Bind<IInMemoryEntityStore>().To<InMemoryEntityStore>().InSingletonScope();
            kernel.Bind<IRepository>().To<RepositorySpy<MemoryRepository>>().InNamedScope("unitofwork");

            // messagebus
            kernel.Bind<HandlerFinder>().ToConstant(new HandlerFinder(
                CocoriCore.Mapper.AssemblyInfo.Assembly,
                CocoriCore.Page.AssemblyInfo.Assembly,
                Comptes.AssemblyInfo.Assembly)).InSingletonScope();
            kernel.Bind<PageMapperConfiguration>().ToConstant(
                new PageMapperConfiguration(
                    Comptes.AssemblyInfo.Assembly)
                );
            kernel.Bind<IPageMapper>().To<PageMapper>().InSingletonScope();
            kernel.Bind<INewMapper>().ToConstant(new NewMapper(Comptes.AssemblyInfo.Assembly));

            kernel.Bind<IMessageBus>().To<MessageBusSpy<Comptes.MessageBus>>().InNamedScope("unitofwork");
            kernel.Bind<IExecuteHandler>().To<ExecuteHandler>().InNamedScope("unitofwork");

            kernel.Bind<IEmailReader, IEmailSender>().To<TestEmailSenderAndReader>().InSingletonScope();

            // claims
            kernel.Bind<TestBrowserClaimsProvider>().ToConstant(new TestBrowserClaimsProvider(response =>
            {
                if (response is IClaimsResponse claimsResponse)
                    return claimsResponse.GetClaims();
                return null;
            }));
            kernel.Bind<IClaimsProvider, IClaimsWriter>().To<ClaimsProviderAndWriter>().InNamedScope("unitofwork");
            kernel.Bind<UserFluentFactory>().ToSelf().DefinesNamedScope("user");
            kernel.Bind<UserFluent>().ToSelf().InNamedScope("user");
            kernel.Bind<ICurrentUserLogger>().To<CurrentUserLogger>().InNamedScope("user");
            kernel.Bind<IBrowser>().To<TestBrowser>().InNamedScope("user");

            kernel.Bind<IUserLogger, UserLogger>().To<UserLogger>().InSingletonScope();

        }

        public void WithSeleniumBrowser(RouterOptions routerOptions)
        {
            kernel.Bind<RouterOptions>().ToConstant(routerOptions);
            kernel.Rebind<IBrowser>().To<SeleniumBrowser>();
        }
        /*
        public BrowserFluent<Accueil_Page> CreateBrowser(string id)
        {
            return kernel.Get<BrowserFluent<int>>().SetId(id).Display(new Accueil_Page_GET());
        }*/

        public UserFluent CreateUser(string userName)
        {
            return kernel.Get<UserFluentFactory>().UserFluent.SetUserName(userName);
        }

        public object[] GetLogs()
        {
            return kernel.Get<UserLogger>().Logs.ToArray();
        }

        public T Get<T>()
        {
            return kernel.Get<T>();
        }

        public T Get<T>(Action<T> modification)
        {
            return kernel.Get<T>();
        }


        /*
        public void Dispose()
        {
            kernel.Dispose();
            //driver.Dispose();
        }
        */
    }
}
