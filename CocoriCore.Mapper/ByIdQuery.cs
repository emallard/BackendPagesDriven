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

            if (typeof(T).IsAssignableToGeneric(typeof(IView<>)))
            {
                var entityType = mapper.GetViewEntityType<T>();
                IEntity entity = (IEntity)await repository.LoadAsync(entityType, message.Id);
                var view = mapper.View<T>(entity);
                return view;
            }
            else if (typeof(T).IsAssignableToGeneric(typeof(IJoin<,>)))
            {
                //(var entityType1, var entityType2) = mapper.GetJoinEntityTypes<T>();
                //IEntity entity1 = (IEntity)await repository.LoadAsync(entityType1, message.Id);
                throw new NotSupportedException(this.GetType().Name);
            }
            else
            {
                return mapper.View<T>();
            }

        }
    }
}