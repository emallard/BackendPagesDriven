using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;

namespace CocoriCore
{
    public class ExtendedRepository : IExtendedRepository
    {
        private readonly INewMapper mapper;
        private readonly IRepository repository;

        public ExtendedRepository(INewMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async Task<Guid> InsertAsync<T>(T t)
        {
            var entity = mapper.CreateEntity(t);
            await repository.InsertAsync(entity);
            return entity.Id;
        }

        public async Task<T> LoadAsync<T>(Guid id)
        {
            var entityType = mapper.GetViewEntityType<T>();
            IEntity entity = entityType == null ? null : (IEntity)await repository.LoadAsync(entityType, id);
            var view = mapper.View<T>(entity);
            return view;
        }

        public async Task<T[]> ToArrayAsync<T>()
        {
            var entityType = mapper.GetViewEntityType<T>();
            var entityArray = await repository.Query(entityType).ToArrayAsync();
            return entityArray.Select(x => mapper.View<T>((IEntity)x)).ToArray();
        }

        public async Task<Guid> UpdateAsync<T>(T t)
        {
            var entityType = mapper.GetUpdateEntityType<T>();
            var idFunc = mapper.GetUpdateIdFunc<T>();
            var idEntity = idFunc(t);
            var entity = (IEntity)await repository.LoadAsync(entityType, idEntity);
            mapper.UpdateEntity(t, entity);
            await repository.UpdateAsync(entity);
            return entity.Id;
        }
    }
}