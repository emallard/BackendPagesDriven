using System;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Ninject;
using Ninject;
using Ninject.Extensions.ContextPreservation;
using Ninject.Extensions.NamedScope;

namespace CocoriCore.Mapper
{
    public class TestBase// : IDisposable
    {
        protected StandardKernel kernel;

        public TestBase()
        {
            kernel = new StandardKernel();
            kernel.Load(new NamedScopeModule());
            kernel.Load(new ContextPreservationModule());
            kernel.Load(new CocoricoreNinjectModule());

            // Repository
            kernel.Bind<IUIDProvider>().To<UIDProvider>().InSingletonScope();
            kernel.Bind<IInMemoryEntityStore>().To<InMemoryEntityStore>().InSingletonScope();
            kernel.Bind<IRepository>().To<MemoryRepository>().InNamedScope("unitofwork");

            kernel.Bind<HandlerFinder>().ToConstant(new HandlerFinder(CocoriCore.Mapper.AssemblyInfo.Assembly)).InSingletonScope();
            kernel.Bind<IMessageBus>().To<CocoriCore.Mapper.MessageBus>().InNamedScope("unitofwork");

            kernel.Bind<INewMapper>().To<NewMapper>().InSingletonScope();

            //kernel.Bind<CocoriCoreModuleLoaderOptions>().ToConstant(new CocoriCoreModuleLoaderOptions(CocoriCore.Mapper.AssemblyInfo.Assembly));
            //kernel.Get<CocoriCoreModuleLoader>();
        }

        protected async Task<T> ExecuteAsync<T>(IMessage<T> message)
        {
            return (T)(await this.ExecuteAsync((IMessage)message));
        }

        protected async Task<object> ExecuteAsync(IMessage message)
        {
            var unitOfWorkFactory = kernel.Get<IUnitOfWorkFactory>();
            using (var unitOfWork = unitOfWorkFactory.NewUnitOfWork())
            {
                //unitOfWork.Resolve<IClaimsWriter>().SetClaims(claims); ;
                var messagebus = unitOfWork.Resolve<IMessageBus>();
                var response = await messagebus.ExecuteAsync(message);

                //var newClaims = browserclaimsProvider.OnResponse(response);
                //if (newClaims != null)
                //    this.claims = newClaims;
                return response;
            }
        }

    }
}