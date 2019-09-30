using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CocoriCore
{
    public interface IPageBase
    {
        void ApplyBindings();

        void AddBinding(string[] from, string[] to);
        void AddInit(string[] from, string[] to);
    }

    public class PageBase<TPageQuery> : IPageBase
    {
        public TPageQuery PageQuery;
        public Type PageType;

        public List<PageBinding> Bindings = new List<PageBinding>();
        public List<PageBinding> Inits = new List<PageBinding>();

        public void ApplyBindings()
        {
            foreach (var b in Bindings)
                ApplyBinding(b);
        }

        private void ApplyBinding(PageBinding binding)
        {
            var value = GetValue(binding.From, this);
            SetValue(binding.To, this, value);
        }

        private object GetValue(string[] memberNames, object o)
        {
            var currentObject = o;
            for (var i = memberNames.Length - 1; i >= 0; --i)
            {
                var memberInfo = currentObject.GetType().GetPropertyOrField(memberNames[i], BindingFlags.Instance | BindingFlags.Public);
                currentObject = memberInfo.InvokeGetter(currentObject);
            }

            return currentObject;
        }

        private void SetValue(string[] memberNames, object o, object value)
        {
            object currentObject = o;
            for (var i = memberNames.Length - 1; i > 0; --i)
            {
                var memberInfo = currentObject.GetType().GetPropertyOrField(memberNames[i], BindingFlags.Instance | BindingFlags.Public);
                currentObject = memberInfo.InvokeGetter(currentObject);
            }

            var memberInfoSet = currentObject.GetType().GetPropertyOrField(memberNames[0], BindingFlags.Instance | BindingFlags.Public);
            memberInfoSet.InvokeSetter(currentObject, value);
        }

        public void AddBinding(string[] from, string[] to)
        {
            Bindings.Add(new PageBinding()
            {
                From = from,
                To = to
            });
        }

        public void AddInit(string[] from, string[] to)
        {
            Inits.Add(new PageBinding()
            {
                From = from,
                To = to
            });
        }

        public void Bind<T>(T page, Expression<Func<T, object>> from, Expression<Func<T, object>> to) where T : IPageBase
        {
            page.AddBinding(GetMemberNames(from.Body), GetMemberNames(to.Body));
        }

        public void Init<T>(T page, Expression<Func<T, object>> from, Expression<Func<T, object>> to) where T : IPageBase
        {
            page.AddInit(GetMemberNames(from.Body), GetMemberNames(to.Body));
        }

        private string[] GetMemberNames(Expression expr)
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

            var names = memberInfos.Select(x => x.Name).ToArray();
            if (names.Length == 0)
                throw new Exception("Error while parsing expression for member names");
            return names;
        }
    }

    public class PageBinding
    {
        public string[] From;
        public string[] To;
    }
}