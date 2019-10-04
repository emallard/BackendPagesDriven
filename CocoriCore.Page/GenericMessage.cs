using System;

namespace CocoriCore
{
    public class GenericMessage : IMessage
    {
        public bool IsCall = true;
        public Type _Type;
        public GenericMessage()
        {
            _Type = this.GetType();
        }
    }
}