using System;

namespace CocoriCore
{
    public interface IAsyncCall
    {
        void SetResult(object o);
        void SetPageQuery(object pageQuery);
    }

    public class AsyncCallDeserializationHelper
    {
        public Type _Type;
        public Type _PageQueryType;
    }

    public class AsyncCall<T> : IMessage<T>, IAsyncCall, ISetPageQuery
    {
        public bool IsAsyncCall = true;
        public Type _Type;
        public Type _PageQueryType;
        public object PageQuery;
        public T Result;

        public AsyncCall()
        {
            _Type = this.GetType();
        }

        public AsyncCall(object pageQuery)
        {
            _Type = this.GetType();
            this.SetPageQuery(pageQuery);
        }

        public void SetResult(object o)
        {
            Result = (T)o;
        }

        public void SetPageQuery(object pageQuery)
        {
            this.PageQuery = pageQuery;
            _PageQueryType = pageQuery.GetType();
        }
    }
}