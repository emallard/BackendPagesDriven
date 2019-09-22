using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore.Page
{
    public interface IBrowser
    {
        Task<T> Follow<TPage, T>(TPage page, Expression<Func<TPage, IMessage<T>>> expressionMessage);

        Task<T> Display<T>(IMessage<T> message);

        Task<TFormResponse> Submit<TPage, TMessage, TFormResponse>(
            TPage page,
            Expression<Func<TPage, Form<TMessage, TFormResponse>>> getForm,
            TMessage message)
        where TMessage : IMessage, new();

        Task<T> SubmitRedirect<T>(IMessage<T> message);
    }
}