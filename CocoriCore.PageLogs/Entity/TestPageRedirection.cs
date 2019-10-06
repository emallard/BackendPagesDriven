using System;

namespace CocoriCore.PageLogs
{
    public class TestPageRedirection : EntityBase<TestPageRedirection>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string ScenarioNames;
        public string FromPageName;

        public string MemberName;
        public bool IsForm;
        public bool IsLink;

        public string ToPageName;
        public bool ToPageHasAssert;

    }
}