using System.Threading.Tasks;

namespace CocoriCore
{
    public static class RepositoryExtension
    {
        public static async Task<T> LoadAsync<T>(this IRepository repository, TypedId<T> id) where T : class, IEntity
        {
            return await repository.LoadAsync<T>(id.Id);
        }
    }
}