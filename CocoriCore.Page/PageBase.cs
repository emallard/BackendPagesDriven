using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CocoriCore
{
    public interface IPageBase
    {
        void ApplyBindings();
    }

    public class PageBase<TPageQuery, TPage> : IPageBase where TPageQuery : IMessage<TPage>
    {
        public TPageQuery PageQuery;

        public List<PageBinding<TPage>> Bindings = new List<PageBinding<TPage>>();

        public void ApplyBindings()
        {
            foreach (var b in Bindings)
                ApplyBinding(b);
        }

        private void ApplyBinding(PageBinding<TPage> binding)
        {
            var value = GetValue(binding.From.Body, this);
            SetValue(binding.To.Body, this, value);
        }

        private object GetValue(Expression expr, object o)
        {
            var expression = expr;
            var memberInfos = new List<MemberInfo>();

            if (expression.NodeType == ExpressionType.Convert)
                expression = ((UnaryExpression)expression).Operand;

            while (expression is MemberExpression memberExpression)
            {
                var memberInfo = memberExpression.Member;
                memberInfos.Add(memberInfo);
                expression = memberExpression.Expression;
            }
            object currentObject = o;
            for (var i = memberInfos.Count - 1; i >= 0; --i)
                currentObject = memberInfos[i].InvokeGetter(currentObject);
            return currentObject;
        }

        private void SetValue(Expression expr, object o, object value)
        {
            var expression = expr;
            var memberInfos = new List<MemberInfo>();

            if (expression.NodeType == ExpressionType.Convert)
                expression = ((UnaryExpression)expression).Operand;

            while (expression is MemberExpression memberExpression)
            {
                var memberInfo = memberExpression.Member;
                memberInfos.Add(memberInfo);
                expression = memberExpression.Expression;
            }

            object currentObject = o;
            for (var i = memberInfos.Count - 1; i > 0; --i)
                currentObject = memberInfos[i].InvokeGetter(currentObject);

            memberInfos[0].InvokeSetter(currentObject, value);
        }

        public void Bind(Expression<Func<TPage, object>> from, Expression<Func<TPage, object>> to)
        {
            Bindings.Add(new PageBinding<TPage>()
            {
                From = from,
                To = to
            });
        }
    }

    public class PageBinding<TPage>
    {
        public Expression<Func<TPage, object>> From;
        public Expression<Func<TPage, object>> To;
    }
}