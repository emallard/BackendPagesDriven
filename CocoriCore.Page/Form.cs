using System;

namespace CocoriCore
{

    public interface IForm
    {
        IMessage GetCommand();
        void SetMemberName(string name);
        string GetMemberName();
    }

    public class Form<TCommand, TResponse> : IForm, IMessage<TResponse>
        where TCommand : IMessage, new()
    {
        public bool IsForm = true;
        public Type _Type;
        public TCommand Command = new TCommand();
        public string MemberName;

        public Form()
        {
            _Type = this.GetType();
        }

        public IMessage GetCommand()
        {
            return Command;
        }

        public string GetMemberName()
        {
            return MemberName;
        }

        public void SetMemberName(string name)
        {
            MemberName = name;
        }
    }
}