using System;
using System.Linq;

namespace CocoriCore
{
    public interface INewMapper
    {
        void AddCreate<T, U>(Action<T, U> action) where T : ICreate<U>;
        void AddUpdate<T, U>(Func<T, TypedId<U>> idFunc, Action<T, U> action) where T : IUpdate<U>;
        void AddView<T, U>(Action<T, U> action) where U : IView<T>;
        void AddView<U>(Action<U> action);

        void AddFrom<T, U, V>(
            Func<T, TypedId<U>> idU,
            Action<T, U> actionU,
            Func<T, TypedId<V>> idV,
            Action<T, V> actionV) where T : IView<U>, IWith<V>;

        IEntity CreateEntity(object o);
        Type GetUpdateEntityType<T>();
        Func<T, Guid> GetUpdateIdFunc<T>();
        void UpdateEntity(object o, object entity);
        Type GetViewEntityType<T>();
        T View<T>(IEntity entity);
        T View<T>();
    }
}