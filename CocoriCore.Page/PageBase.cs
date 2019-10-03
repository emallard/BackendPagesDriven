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
    }

    public class PageBase<TPageQuery> : IPageBase
    {
        public TPageQuery PageQuery;
        public string PageTypeName;

        public List<PageBinding> OnSubmits = new List<PageBinding>();
        public List<PageBinding> OnInits = new List<PageBinding>();
        public List<PageBinding> Inputs = new List<PageBinding>();

        public void ApplyBindings()
        {
            foreach (var b in OnSubmits)
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
            for (var i = 0; i < memberNames.Length; i++)
            {
                var memberInfo = currentObject.GetType().GetPropertyOrField(memberNames[i], BindingFlags.Instance | BindingFlags.Public);
                currentObject = memberInfo.InvokeGetter(currentObject);
            }

            return currentObject;
        }

        private void SetValue(string[] memberNames, object o, object value)
        {
            object currentObject = o;
            for (var i = 0; i < memberNames.Length - 1; i++)
            {
                var memberInfo = currentObject.GetType().GetPropertyOrField(memberNames[i], BindingFlags.Instance | BindingFlags.Public);
                currentObject = memberInfo.InvokeGetter(currentObject);
            }

            var memberInfoSet = currentObject.GetType().GetPropertyOrField(memberNames.Last(), BindingFlags.Instance | BindingFlags.Public);
            memberInfoSet.InvokeSetter(currentObject, value);
        }

        public void OnSubmit<T>(T page, Expression<Func<T, object>> from, Expression<Func<T, object>> to) where T : IPageBase
        {
            OnSubmits.Add(new PageBinding()
            {
                From = GetMemberNames(from.Body),
                To = GetMemberNames(to.Body)
            });
        }

        public void OnInit<T>(T page, Expression<Func<T, object>> from, Expression<Func<T, object>> to) where T : IPageBase
        {
            OnInits.Add(new PageBinding()
            {
                From = GetMemberNames(from.Body),
                To = GetMemberNames(to.Body)
            });
        }

        public void Render<T>(T page, Expression<Func<T, object>> member, Expression<Func<T, object>> form) where T : IPageBase
        {
            Inputs.Add(new PageBinding()
            {
                From = GetMemberNames(member.Body),
                To = GetMemberNames(form.Body)
            });
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

            memberInfos.Reverse();

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