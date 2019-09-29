using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore
{
    public class NewMapper : INewMapper
    {
        public List<NewMapperMapping> CreateMappings = new List<NewMapperMapping>();
        public List<NewMapperMapping> ViewMappings = new List<NewMapperMapping>();
        public List<NewMapperUpdateMapping> UpdateMappings = new List<NewMapperUpdateMapping>();

        public NewMapper(params Assembly[] assemblies)
        {
            var moduleTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => t.IsConcrete() && t.IsAssignableTo(typeof(INewMapperModule)))).ToArray();
            var modules = moduleTypes.Select(t => Activator.CreateInstance(t)).Cast<INewMapperModule>().ToArray();
            foreach (var m in modules)
            {
                m.Load(this);
            }
        }

        public NewMapper(params INewMapperModule[] modules)
        {
            foreach (var m in modules)
            {
                m.Load(this);
            }
        }

        public void AddCreate<T, U>(Action<T, U> action) where T : ICreate<U>
        {
            CreateMappings.Add(new NewMapperMapping()
            {
                FromType = typeof(T),
                ToType = typeof(U),
                MapAction = (a, b) => action((T)a, (U)b)
            });
        }

        public void AddUpdate<T, U>(Func<T, ID<U>> idFunc, Action<T, U> action) where T : IUpdate<U>
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

        public void AddView<U>(Action<U> action)
        {
            ViewMappings.Add(new NewMapperMapping()
            {
                FromType = null,
                ToType = typeof(U),
                MapAction = (a, b) => action((U)b)
            });
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

        public T View<T>()
        {
            var view = Activator.CreateInstance<T>();
            var mapping = ViewMappings.First(x => x.ToType == typeof(T) && x.FromType == null);
            mapping.MapAction(null, view);
            return view;
        }

        public void AddJoin<T, U, V>(Func<T, ID<U>> idU, Action<T, U> actionU, Func<T, ID<V>> idV, Action<T, V> actionV) where T : IJoin<U, V>
        {
            throw new NotImplementedException();
        }

    }
}