using System;

namespace CocoriCore.PageLogs
{
    public class TestPage : EntityBase<TestPage>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string PageName;

        public bool HasAssert = false;
        //public string Url;
    }
}