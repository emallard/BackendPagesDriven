using System;

namespace CocoriCore
{
    public class HandlerFunc
    {
        public Type Key;
        public Func<object, object> Func { get; set; }

        public static HandlerFunc Create<T1, T2>(Func<T1, T2> func)
        {
            return new HandlerFunc()
            {
                Func = (a) => func((T1)a),
                Key = typeof(T1)
            };
        }
    }
}