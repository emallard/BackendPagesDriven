using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoriCore
{
    public interface INewMapper
    {
        void AddCreate<T, U>(Action<T, U> action) where T : ICreate<U>;
        void AddUpdate<T, U>(Func<T, TypedId<U>> idFunc, Action<T, U> action) where T : IUpdate<U>;
        void AddView<T, U>(Action<T, U> action) where U : IView<T>;
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
    }

    public class NewMapperMapping
    {
        public Type FromType;
        public Type ToType;
        public Action<object, object> MapAction;

    }

    public class NewMapperUpdateMapping
    {
        public Func<object, Guid> IdFunc;
        public Type FromType;
        public Type ToType;
        public Action<object, object> MapAction;
    }

    public class NewMapper : INewMapper
    {
        public List<NewMapperMapping> CreateMappings = new List<NewMapperMapping>();
        public List<NewMapperMapping> ViewMappings = new List<NewMapperMapping>();
        public List<NewMapperUpdateMapping> UpdateMappings = new List<NewMapperUpdateMapping>();


        public void AddCreate<T, U>(Action<T, U> action) where T : ICreate<U>
        {
            CreateMappings.Add(new NewMapperMapping()
            {
                FromType = typeof(T),
                ToType = typeof(U),
                MapAction = (a, b) => action((T)a, (U)b)
            });
        }

        public void AddUpdate<T, U>(Func<T, TypedId<U>> idFunc, Action<T, U> action) where T : IUpdate<U>
        {
            UpdateMappings.Add(new NewMapperUpdateMapping()
            {
                IdFunc = a => (idFunc((T)a)).Id,
                FromType = typeof(T),
                ToType = typeof(U),
                MapAction = (a, b) => action((T)a, (U)b)
            });
        }


        public void AddView<T, U>(Action<T, U> action) where U : IView<T>
        {
            ViewMappings.Add(new NewMapperMapping()
            {
                FromType = typeof(T),
                ToType = typeof(U),
                MapAction = (a, b) => action((T)a, (U)b)
            });
        }

        public void AddFrom<T, U, V>(Func<T, TypedId<U>> idU, Action<T, U> actionU, Func<T, TypedId<V>> idV, Action<T, V> actionV) where T : IView<U>, IWith<V>
        {
            throw new NotImplementedException();
        }


        public IEntity CreateEntity(object o)
        {
            var mapping = CreateMappings.First(x => x.FromType == o.GetType());
            var entity = Activator.CreateInstance(mapping.ToType);
            mapping.MapAction(o, entity);
            return (IEntity)entity;
        }

        public Type GetUpdateEntityType<T>()
        {
            var mapping = UpdateMappings.First(x => x.FromType == typeof(T));
            return mapping.ToType;
        }



        public Func<T, Guid> GetUpdateIdFunc<T>()
        {
            var mapping = UpdateMappings.First(x => x.FromType == typeof(T));
            return t => (Guid)mapping.IdFunc(t);
        }

        public void UpdateEntity(object o, object entity)
        {
            var mapping = UpdateMappings.First(x => x.ToType == entity.GetType() && x.FromType == o.GetType());
            mapping.MapAction(o, entity);
        }

        public Type GetViewEntityType<T>()
        {
            var mapping = ViewMappings.First(x => x.ToType == typeof(T));
            return mapping.FromType;
        }

        public T View<T>(IEntity entity)
        {
            var view = Activator.CreateInstance<T>();
            var mapping = ViewMappings.First(x => x.ToType == typeof(T) && x.FromType == entity.GetType());
            mapping.MapAction(entity, view);
            return view;
        }
    }
}