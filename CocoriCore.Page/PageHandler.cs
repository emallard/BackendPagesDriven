using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore
{
    public abstract class PageHandler<TQuery, TPage> : IHandler<TQuery, TPage>
        where TQuery : IMessage<TPage>
        where TPage : new()
    {
        TQuery currentMessage;
        protected TPage Page;

        public async Task<object> HandleAsync(IMessage message)
        {
            await Task.CompletedTask;
            currentMessage = (TQuery)message;
            Page = new TPage();
            ExecuteAsync((TQuery)message);
            return Page;
        }

        public abstract void ExecuteAsync(TQuery message);


        protected void Create<TMessage, TMessageResponse, TCallResponse>(
            Expression<Func<TPage, PageCall<TQuery, TMessage, TMessageResponse, TCallResponse>>> expression,
            TMessage message,
            Func<TMessageResponse, TCallResponse> translate
        )
        {
            var body = (MemberExpression)expression.Body;
            var memberInfo = body.Member;

            var pageCall = new PageCall<TQuery, TMessage, TMessageResponse, TCallResponse>();
            pageCall.PageMessage = this.currentMessage;
            pageCall.MemberName = memberInfo.Name;
            pageCall.Message = message;
            pageCall.Translate = (m, r) => translate(r);

            memberInfo.InvokeSetter(Page, pageCall);
        }
    }
}