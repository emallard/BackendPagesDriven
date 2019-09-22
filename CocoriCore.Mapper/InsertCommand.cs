using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class InsertCommand<T> : IMessage<Guid>
    {
        public T Object;
    }

    public class InsertCommandHandler<T> : IHandler<InsertCommand<T>, Guid>
    {
        private readonly IRepository repository;
        private readonly INewMapper mapper;

        public InsertCommandHandler(IRepository repository, INewMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Guid> ExecuteAsync(InsertCommand<T> message)
        {
            var entity = mapper.CreateEntity(message.Object);
            await repository.InsertAsync(entity);
            return entity.Id;

        }

        public async Task<object> HandleAsync(IMessage message)
        {
            return await ExecuteAsync((InsertCommand<T>)message);
        }
    }
}