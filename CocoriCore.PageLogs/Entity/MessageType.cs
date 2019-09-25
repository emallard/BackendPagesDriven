using System;

namespace CocoriCore.PageLogs
{
    public class MessageType : EntityBase<MessageType>
    {
        public string Name;
        public Type MessageType_;
        public Type ResponseType;
    }
}