using System;

namespace CocoriCore
{
    public class PageMapping3
    {
        public Tuple<Type, Type, Type> Key { get; set; }
        public Func<object, object, object> Func { get; set; }

        public static PageMapping3 Create<T1, T2, T3>(Func<T1, T2, T3> func)
        {
            return new PageMapping3()
            {
                Func = (a, b) => func((T1)a, (T2)b),
                Key = Tuple.Create(typeof(T1), typeof(T2), typeof(T3))
            };
        }
    }

    public class PageMapping2
    {
        public Tuple<Type, Type> Key { get; set; }
        public Func<object, object> Func { get; set; }

        public static PageMapping2 Create<T1, T2>(Func<T1, T2> func)
        {
            return new PageMapping2()
            {
                Func = (a) => func((T1)a),
                Key = Tuple.Create(typeof(T1), typeof(T2))
            };
        }
    }
}
