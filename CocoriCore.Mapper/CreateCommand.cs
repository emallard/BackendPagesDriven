using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class CreateCommand<T> : IMessage<Guid>
    {
        public T Object;
    }

    public class CreateCommandHandler<T> : MessageHandler<CreateCommand<T>, Guid>
    {
        private readonly IRepository repository;
        private readonly INewMapper mapper;

        public CreateCommandHandler(IRepository repository, INewMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<Guid> ExecuteAsync(CreateCommand<T> message)
        {
            var entity = mapper.CreateEntity(message.Object);
            await repository.InsertAsync(entity);
            return entity.Id;

        }
    }
}