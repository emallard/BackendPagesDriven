using System;

namespace CocoriCore.PageLogs
{
    public class TestPage : EntityBase<TestPage>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string ScenarioNames;
        public string PageName;

        public bool HasAssert = false;
        public string PageUrl;
        //public string Url;
    }
}