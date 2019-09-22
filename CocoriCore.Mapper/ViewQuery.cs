using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class ViewQuery<T> : IMessage<T>
    {
        public Guid Id;
    }

    public class ViewQueryHandler<T> : IHandler<ViewQuery<T>, T>
    {
        private readonly IRepository repository;
        private readonly INewMapper mapper;

        public ViewQueryHandler(IRepository repository, INewMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<T> ExecuteAsync(ViewQuery<T> message)
        {
            var entityType = mapper.GetViewEntityType<T>();
            var entity = (IEntity)await repository.LoadAsync(entityType, message.Id);
            var view = mapper.View<T>(entity);
            return view;

        }

        public async Task<object> HandleAsync(IMessage message)
        {
            return await ExecuteAsync((ViewQuery<T>)message);
        }
    }
}