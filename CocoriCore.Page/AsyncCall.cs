using System;

namespace CocoriCore
{
    public interface IAsyncCall
    {
        void SetResult(object o);
    }

    public class AsyncCall<TPageGet, T> : IMessage<T>, IAsyncCall
    {
        public bool IsAsyncCall = true;
        public Type _Type;
        public TPageGet PageQuery;
        public T Result;

        public AsyncCall()
        {
            _Type = this.GetType();
        }

        public void SetResult(object o)
        {
            Result = (T)o;
        }
    }
}