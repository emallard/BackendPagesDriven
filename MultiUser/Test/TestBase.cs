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

namespace MultiUser
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
            kernel.Load(new CocoricorePageNinjectModule());

            kernel.Bind<IHashService>().To<HashService>().InSingletonScope();
            kernel.Bind<IClock, SettableClock>().To<SettableClock>().InSingletonScope();

            // Repository
            kernel.Bind<IUIDProvider>().To<UIDProvider>().InSingletonScope();
            kernel.Bind<IInMemoryEntityStore>().To<InMemoryEntityStore>().InSingletonScope();
            kernel.Bind<IRepository>().To<RepositorySpy<MemoryRepository>>().InNamedScope("unitofwork");

            kernel.Bind<IMessageBus>().To<MessageBusSpy<MultiUser.MessageBus>>().InNamedScope("unitofwork");

            // messagebus
            kernel.Bind<HandlerFinder>().ToConstant(new HandlerFinder(
                CocoriCore.Page.AssemblyInfo.Assembly,
                MultiUser.AssemblyInfo.Assembly)).InSingletonScope();
            kernel.Bind<PageMapperConfiguration>().ToConstant(
                new PageMapperConfiguration(
                    MultiUser.AssemblyInfo.Assembly)
                );


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
