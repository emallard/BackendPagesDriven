using System;

namespace CocoriCore
{
    public class PageHandling
    {
        public Type Key;
        public Func<object, object> Func { get; set; }

        public static PageHandling Create<T1, T2>(Func<T1, T2> func)
        {
            return new PageHandling()
            {
                Func = (a) => func((T1)a),
                Key = typeof(T1)
            };
        }
    }
}