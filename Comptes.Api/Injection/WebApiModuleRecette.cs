
using CocoriCore;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;

namespace Comptes.Api
{
    public class WebApiModuleRecette : NinjectModule
    {
        public override void Load()
        {
            this.Bind<InMemoryEntityStore>().ToSelf().InSingletonScope();
            this.Bind<MemoryRepository>().ToSelf().InSingletonScope();
            this.Bind<IRepository>().To<MemoryRepository>().InNamedScope("unitofwork");
        }
    }
}
