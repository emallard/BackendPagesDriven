using System;

namespace CocoriCore
{
    public interface IAsyncCall
    {
        void SetResult(object o);
        void SetPageQuery(object pageQuery);
        void SetMemberName(string name);
        string GetMemberName();
        IMessage GetMessage();

        AsyncCallType GetCallType();
    }

    public enum AsyncCallType
    {
        OnInit,
        Action,
        Server
    }


    public class AsyncCall<TMessage, TModel> : IMessage<TModel>, IAsyncCall where TMessage : IMessage
    {
        public bool IsAsyncCall = true;
        public AsyncCallType CallType;
        public Type _Type;
        public Type _PageQueryType;
        public TMessage Message = Activator.CreateInstance<TMessage>();
        public TModel Result;
        public string MemberName;

        public AsyncCall()
        {
            _Type = this.GetType();
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

        public IMessage GetMessage()
        {
            return Message;
        }

        public string GetMemberName()
        {
            return MemberName;
        }

        public AsyncCallType GetCallType()
        {
            return this.CallType;
        }
    }
}