using System;

namespace CocoriCore
{
    public interface IAsyncCall
    {
        void SetResult(object o);
        void SetPageQuery(object pageQuery);
        void SetMemberName(string name);
        string GetMemberName();
        IMessage GetQuery();
    }

    public class AsyncCall<TQuery, TModel> : IMessage<TModel>, IAsyncCall where TQuery : IMessage
    {
        public bool IsAsyncCall = true;
        public bool OnInit = true;
        public Type _Type;
        public Type _PageQueryType;
        public TQuery Query = Activator.CreateInstance<TQuery>();
        public TModel Result;
        public string MemberName;

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
            Result = (TModel)o;
        }

        public void SetPageQuery(object pageQuery)
        {
            _PageQueryType = pageQuery.GetType();
        }

        public void SetMemberName(string name)
        {
            MemberName = name;
        }

        public IMessage GetQuery()
        {
            return Query;
        }

        public string GetMemberName()
        {
            return MemberName;
        }
    }
}