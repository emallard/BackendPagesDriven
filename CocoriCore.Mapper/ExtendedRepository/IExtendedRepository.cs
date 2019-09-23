using System;
using System.Linq;
using System.Threading.Tasks;

namespace CocoriCore
{

    public interface IExtendedRepository
    {
        Task<T> LoadAsync<T>(Guid id);

        Task<Guid> InsertAsync<T>(T t);

        Task<Guid> UpdateAsync<T>(T t);

        Task<T[]> ToArrayAsync<T>();
    }
}