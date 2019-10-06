using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore.Page
{

    public class TestBrowser : IBrowser
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly TestBrowserClaimsProvider browserclaimsProvider;
        private IClaims claims;

        public TestBrowser(
            IUnitOfWorkFactory unitOfWorkFactory,
            TestBrowserClaimsProvider browserclaimsProvider)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.browserclaimsProvider = browserclaimsProvider;
        }

        public async Task<T> Follow<TPage, T>(TPage page, Expression<Func<TPage, IMessage<T>>> expressionMessage) where T : IPageBase
        {
            var func = expressionMessage.Compile();
            var message = func(page);
            var next = await this.ExecuteAsync(message);
            await ExecuteAsyncCalls(next);
            return next;
        }

        public async Task<T> Display<T>(IMessage<T> message) where T : IPageBase
        {
            var next = await this.ExecuteAsync(message);
            await ExecuteAsyncCalls(next);
            return next;
        }

        public async Task<T> SubmitRedirect<T>(IMessage<T> message) where T : IPageBase
        {
            var next = await this.ExecuteAsync(message);
            await ExecuteAsyncCalls(next);
            return next;
        }

        private async Task ExecuteAsyncCalls<T>(T page) where T : IPageBase
        {
            await ExecuteAsyncCalls(page, page);
            page.ApplyOnInits();
        }
        private async Task ExecuteAsyncCalls<T>(T page, object o)
        {
            if (o == null)
                return;
            var mis = o.GetType().GetPropertiesAndFields();
            foreach (var mi in mis)
            {
                var memberType = mi.GetMemberType();
                if (memberType.IsAssignableTo<IAsyncCall>())
                {
                    var asyncCall = (IAsyncCall)mi.InvokeGetter(o);
                    asyncCall.SetResult(await ExecuteAsync((IMessage)mi.InvokeGetter(o)));
                }
                else
                {
                    if (memberType != typeof(string)
                     && !memberType.IsValueType
                     && memberType != typeof(Type)
                     && memberType != typeof(Assembly)
                     && !memberType.IsAssignableTo(typeof(IEnumerable))
                    )
                    {
                        await ExecuteAsyncCalls(page, mi.InvokeGetter(o));
                    }
                }
            }
        }

        private async Task<T> ExecuteAsync<T>(IMessage<T> message)
        {
            return (T)(await this.ExecuteAsync((IMessage)message));
        }

        private async Task<object> ExecuteAsync(IMessage message)
        {
            using (var unitOfWork = unitOfWorkFactory.NewUnitOfWork())
            {
                unitOfWork.Resolve<IClaimsWriter>().SetClaims(claims); ;
                var messagebus = unitOfWork.Resolve<IMessageBus>();
                var response = await messagebus.ExecuteAsync(message);

                var newClaims = browserclaimsProvider.OnResponse(response);
                if (newClaims != null)
                    this.claims = newClaims;
                return response;
            }
        }

        /*/
        public async Task<TFormResponse> Submit<TPage, TMessage, TFormResponse>(TPage page, Expression<Func<TPage, Form<TMessage, TFormResponse>>> expressionForm, TMessage message) where TMessage : IMessage, new()
        {
            var func = expressionForm.Compile();
            var form = func(page);
            form.Command = message;
            return await ExecuteAsync(form);
        }*/

        public void ApplyOnSubmits(IPageBase page)
        {
            page.ApplyOnSubmits();
        }

        public void ApplyOnInits(IPageBase page)
        {
            page.ApplyOnInits();
        }

        public async Task<TFormResponse> Submit<TPage, TMessage, TFormResponse>(TPage page, Expression<Func<TPage, Form<TMessage, TFormResponse>>> expressionForm) where TMessage : IMessage, new()
        {
            var func = expressionForm.Compile();
            var form = func(page);
            return await ExecuteAsync(form);
        }

        public void Fill<TPage, TMember>(TPage page, Expression<Func<TPage, TMember>> expressionMember, TMember value)
        {
            var expression = expressionMember.Body;
            var memberInfos = new List<MemberInfo>();
            while (expression is MemberExpression memberExpression)
            {
                var memberInfo = memberExpression.Member;
                memberInfos.Add(memberInfo);
                expression = memberExpression.Expression;
            }

            object currentObject = page;
            for (var i = memberInfos.Count - 1; i > 0; --i)
                currentObject = memberInfos[i].InvokeGetter(currentObject);

            memberInfos[0].InvokeSetter(currentObject, value);
        }
    }
}
