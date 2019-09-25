using System;

namespace CocoriCore.PageLogs
{

    public class TestMessage : EntityBase<TestMessage>
    {
        public TId<Test> TestId;
        public TId<TestUser> UserId;

        public TId<TestPage> PageId;
        public TId<PageType> PageTypeId;

        public TId<MessageType> MessageTypeId;
        public object Message;
        public object Response;
    }
}
