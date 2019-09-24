using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class ByIdQuery<T> : IMessage<T>
    {
        public Guid Id;
    }

    public class ByIdQueryHandler<T> : MessageHandler<ByIdQuery<T>, T>
    {
        private readonly IRepository repository;
        private readonly INewMapper mapper;

        public ByIdQueryHandler(IRepository repository, INewMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<T> ExecuteAsync(ByIdQuery<T> message)
        {
            var entityType = mapper.GetViewEntityType<T>();
            if (entityType == null)
                return mapper.View<T>();

            IEntity entity = (IEntity)await repository.LoadAsync(entityType, message.Id);
            var view = mapper.View<T>(entity);
            return view;

        }
    }
}