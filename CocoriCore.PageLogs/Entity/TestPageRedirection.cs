namespace CocoriCore.PageLogs
{
    public class TestPageRedirection : EntityBase<TestPageRedirection>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string PageName;

        public string MemberName;
        public string ToPageName;
        public bool IsForm;
        public bool IsLink;
    }
}