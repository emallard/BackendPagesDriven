using System;
using System.Linq.Expressions;

namespace CocoriCore
{
    public class PageCallMessage<TPage, TPageGet, TMessage, TMessageResponse, TFormResponse> : IMessage<TFormResponse>
        where TPageGet : IMessage
    {
        public IMessage<TPage> PageGet;
        public Func<TPage, PageCall<TPageGet, TMessage, TMessageResponse, TFormResponse>> PageMember;
        public TMessage Message;
    }
}