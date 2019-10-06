using System;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class TestRepositoryOperation : EntityBase<TestRepositoryOperation>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string ScenarioNames;
        public string PageName;
        public string MessageName;
        public string EntityName;
        public LogRepoOperation Operation;

        /*
        public TId<Test> TestId;
        public TId<TestUser> TestUserId;

        public TId<TestPage> TestPageId;
        public TId<PageType> PageTypeId;


        public TId<TestMessage> TestMessageId;
        public TId<MessageType> MessageTypeId;

        public TId<EntityType> EntityTypeId;

        public bool Read;
        public bool Write;
        public bool Update;
        public bool Delete;
        */
    }
}