using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CocoriCore.Page
{
    public class RepositorySpy<T> : IRepository where T : IRepository
    {
        private T repo;
        private readonly ICurrentUserLogger logger;

        public RepositorySpy(
            T repo,
            ICurrentUserLogger logger)
        {
            this.repo = repo;
            this.logger = logger;
        }
        public void Dispose()
        {
            repo.Dispose();
        }

        public async Task<bool> ExistsAsync(Type entityType, Guid id)
        {
            logger.Log(new LogRepo(LogRepoOperation.Exists, entityType));
            return await repo.ExistsAsync(entityType, id);
        }

        public async Task<bool> ExistsAsync(Type entityType, MemberInfo member, object value)
        {
            logger.Log(new LogRepo(LogRepoOperation.Exists, entityType));
            return await repo.ExistsAsync(entityType, member, value);
        }

        public async Task<bool> ExistsAsync(Type entityType, IEnumerable<Guid> collection)
        {
            logger.Log(new LogRepo(LogRepoOperation.Exists, entityType));
            return await repo.ExistsAsync(entityType, collection);
        }

        public async Task<object> LoadAsync(Type type, Guid id)
        {
            logger.Log(new LogRepo(LogRepoOperation.Load, type));
            return await repo.LoadAsync(type, id);
        }

        public IQueryable<object> Query(Type entityType)
        {
            logger.Log(new LogRepo(LogRepoOperation.Load, entityType));
            return repo.Query(entityType);
        }

        async Task IRepository.DeleteAsync<TEntity>(TEntity entity)
        {
            logger.Log(new LogRepo(LogRepoOperation.Delete, entity.GetType()));
            await repo.DeleteAsync<TEntity>(entity);
        }

        async Task<bool> IRepository.ExistsAsync<TEntity>(Guid id)
        {
            logger.Log(new LogRepo(LogRepoOperation.Exists, typeof(TEntity)));
            return await repo.ExistsAsync<TEntity>(id);
        }

        async Task IRepository.InsertAsync<TEntity>(TEntity entity)
        {
            logger.Log(new LogRepo(LogRepoOperation.Insert, entity.GetType()));
            await repo.InsertAsync<TEntity>(entity);
        }

        async Task<TEntity> IRepository.LoadAsync<TEntity>(Guid id)
        {
            logger.Log(new LogRepo(LogRepoOperation.Load, typeof(TEntity)));
            return await repo.LoadAsync<TEntity>(id);
        }

        async Task<TEntity> IRepository.LoadAsync<TEntity>(Expression<Func<TEntity, object>> uniqueMember, object value)
        {
            logger.Log(new LogRepo(LogRepoOperation.Load, typeof(TEntity)));
            return await repo.LoadAsync<TEntity>(uniqueMember, value);
        }

        IQueryable<TEntity> IRepository.Query<TEntity>()
        {
            logger.Log(new LogRepo(LogRepoOperation.Query, typeof(TEntity)));
            return repo.Query<TEntity>();
        }

        async Task IRepository.UpdateAsync<TEntity>(TEntity entity)
        {
            logger.Log(new LogRepo(LogRepoOperation.Update, entity.GetType()));
            await repo.UpdateAsync(entity);
        }

        async Task IRepository.UpdateAsync(object entity)
        {
            logger.Log(new LogRepo(LogRepoOperation.Update, entity.GetType()));
            await repo.UpdateAsync(entity);
        }
    }
}
