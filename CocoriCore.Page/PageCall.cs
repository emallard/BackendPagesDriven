using System;
using Newtonsoft.Json;

namespace CocoriCore
{
    public interface IPageCall
    {
    }

    public class PageCall<TPageMessage, TMessage, TResponse, TPageResponse>
            : Call
            , IMessage<TPageResponse>
        where TPageMessage : IMessage

    {
        public TPageMessage PageMessage;
        public string MemberName;
        public TMessage Message;
        [JsonIgnore]
        public Func<TMessage, TResponse, TPageResponse> Translate;
    }

}