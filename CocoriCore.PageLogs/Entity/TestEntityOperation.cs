using System;

namespace CocoriCore.PageLogs
{
    public enum TestEntityOperationType
    {
        Read,
        Create,
        Update,
        Delete
    }

    public class TestEntityOperation : EntityBase<TestEntityOperation>
    {
        public TId<Test> TestId;
        public TId<TestUser> TestUserId;

        public TId<TestPage> TestPageId;
        public TId<PageType> PageTypeId;


        public TId<TestMessage> TestMessageId;
        public TId<MessageType> MessageTypeId;

        public TId<EntityType> EntityTypeId;

        TestEntityOperationType Type;
        /*
        public bool Read;
        public bool Write;
        public bool Update;
        public bool Delete;
        */
    }
}