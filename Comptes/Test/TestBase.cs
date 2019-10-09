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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.CompilerServices;

namespace Comptes
{
    public class TestBase : IPageTest// : IDisposable
    {
        private StandardKernel kernel;
        private string filePath;
        private int lineNumber;

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
                CocoriCore.Page.AssemblyInfo.Assembly,
                Comptes.AssemblyInfo.Assembly)).InSingletonScope();
            kernel.Bind<PageMapperConfiguration>().ToConstant(
                new PageMapperConfiguration(
                    Comptes.AssemblyInfo.Assembly)
                );
            kernel.Bind<IPageMapper>().To<PageMapper>().InSingletonScope();
            //kernel.Bind<INewMapper>().ToConstant(new NewMapper(Comptes.AssemblyInfo.Assembly));

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
            kernel.Bind<JsonSerializer>().ToMethod(ctx =>
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new StringEnumConverter());
                serializer.Converters.Add(new IDConverter());
                return serializer;
            }).InSingletonScope();
            kernel.Bind<RouterOptions>().ToConstant(routerOptions);
            kernel.Rebind<IBrowser>().To<SeleniumBrowser>().InNamedScope("user");
        }

        public UserFluent CreateUser(string userName,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            this.filePath = file;
            this.lineNumber = line;
            return kernel.Get<UserFluentFactory>().UserFluent.SetUserName(userName);
        }

        /*
        private static void Log0()
        {
            string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
        }
        private static void Log(string text,
                        [CallerFilePath] string file = "",
                        [CallerMemberName] string member = "",
                        [CallerLineNumber] int line = 0)
        {
            Console.WriteLine("{0}_{1}({2}): {3}", Path.GetFileName(file), member, line, text);
        }*/

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
            var x = kernel.Get<T>();
            modification(x);
            return x;
        }

        public string GetFilePath()
        {
            return this.filePath;
        }

        public int GetLineNumber()
        {
            return this.lineNumber;
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
