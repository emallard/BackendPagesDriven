using System;

namespace CocoriCore.PageLogs
{

    public class TestMessage : EntityBase<TestMessage>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string PageName;
        public string MessageName;
        public string ResponseName;
    }
}
