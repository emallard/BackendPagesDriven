using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CocoriCore
{
    public interface IPageBase
    {
        void ApplyOnSubmits();
        void ApplyOnInits();
    }

    public class PageBase<TPageQuery> : IPageBase
    {
        public TPageQuery PageQuery;
        public string PageTypeName;

        public List<PageBinding> OnSubmits = new List<PageBinding>();
        public List<PageBinding> OnInits = new List<PageBinding>();

        public void ApplyOnSubmits()
        {
            foreach (var b in OnSubmits)
                ApplyBinding(b);
        }

        public void ApplyOnInits()
        {
            foreach (var b in OnInits)
                TryApplyBinding(b);
        }

        private void ApplyBinding(PageBinding binding)
        {
            object value;
            if (TryGetValue(binding.From, this, out value))
                SetValue(binding.To, this, value);
            else
                throw new Exception("Binding can't be applied");
        }

        private void TryApplyBinding(PageBinding binding)
        {
            object value;
            if (TryGetValue(binding.From, this, out value))
                SetValue(binding.To, this, value);
        }

        private bool TryGetValue(string[] memberNames, object o, out object value)
        {
            value = null;
            var currentObject = o;
            for (var i = 0; i < memberNames.Length; i++)
            {
                if (currentObject == null)
                    return false;
                var memberInfo = currentObject.GetType().GetPropertyOrField(memberNames[i], BindingFlags.Instance | BindingFlags.Public);
                currentObject = memberInfo.InvokeGetter(currentObject);
            }

            value = currentObject;
            return true;
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

        public void OnSubmit<TPage, TMember>(TPage page, Expression<Func<TPage, TMember>> from, Expression<Func<TPage, TMember>> to)
            where TPage : IPageBase
        {
            OnSubmits.Add(new PageBinding()
            {
                From = GetMemberNames(from.Body),
                To = GetMemberNames(to.Body)
            });
        }

        public void OnInit<TPage, TMember>(TPage page, Expression<Func<TPage, TMember>> from, Expression<Func<TPage, TMember>> to)
            where TPage : IPageBase
        {
            OnInits.Add(new PageBinding()
            {
                From = GetMemberNames(from.Body),
                To = GetMemberNames(to.Body)
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