using System;

namespace CocoriCore
{



    public class Call<TMessage, TResponse>
        : GenericMessage
        , IMessage<TResponse>
        where TMessage : IMessage<TResponse>, new()
    {
        public TMessage Message;
        public Call()
        {
            Message = new TMessage();
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