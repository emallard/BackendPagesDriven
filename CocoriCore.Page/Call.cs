using System;

namespace CocoriCore
{

    public class Call : IMessage
    {
        public bool IsCall = true;
        public Type _Type;
        public Call()
        {
            _Type = this.GetType();
        }
    }



    public class Call<TMessage, TResponse>
        : Call
        , IMessage<TResponse>
        where TMessage : IMessage<TResponse>
    {
        public TMessage Message;

        public Call(TMessage message) : base()
        {
            Message = message;
        }

        public object GetMessage()
        {
            return this.Message;
        }
        public Type GetMessageType()
        {
            return typeof(TMessage);
        }

        public Type GetResponseType()
        {
            return typeof(TResponse);
        }
    }
}