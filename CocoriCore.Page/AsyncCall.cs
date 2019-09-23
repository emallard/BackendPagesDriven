using System;

namespace CocoriCore
{
    public interface IAsyncCall
    {
        void SetResult(object o);
    }

    public class AsyncCall<T> : IMessage<T>, IAsyncCall
    {
        public bool IsAsyncCall = true;
        public Type _Type;
        public object PageQuery;
        public T Result;

        public AsyncCall()
        {
            _Type = this.GetType();
        }

        public AsyncCall(object pageQuery)
        {
            _Type = this.GetType();
            PageQuery = pageQuery;
        }

        public void SetResult(object o)
        {
            Result = (T)o;
        }
    }
}