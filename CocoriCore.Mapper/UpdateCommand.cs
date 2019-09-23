using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class UpdateCommand<T> : IMessage<Guid>
    {
        public T Object;
    }

    public class UpdateCommandHandler<T> : MessageHandler<UpdateCommand<T>, Guid>
    {
        private readonly IRepository repository;
        private readonly INewMapper mapper;

        public UpdateCommandHandler(IRepository repository, INewMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<Guid> ExecuteAsync(UpdateCommand<T> message)
        {
            var entityType = mapper.GetUpdateEntityType<T>();
            var idFunc = mapper.GetUpdateIdFunc<T>();
            var idEntity = idFunc(message.Object);
            var entity = (IEntity)await repository.LoadAsync(entityType, idEntity);
            mapper.UpdateEntity(message.Object, entity);
            await repository.UpdateAsync(entity);
            return entity.Id;
        }
    }
}