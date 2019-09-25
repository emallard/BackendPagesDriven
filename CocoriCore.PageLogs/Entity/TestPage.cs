using System;

namespace CocoriCore.PageLogs
{
    public class TestPage : EntityBase<TestPage>
    {
        public TId<Test> TestId;
        public TId<TestUser> UserId;
        public TId<PageType> PageTypeId;
        public object PageQuery;
        public string Url;
        public bool HasAsserts;
        public TId<TestPage> PreviousPageId;
    }
}