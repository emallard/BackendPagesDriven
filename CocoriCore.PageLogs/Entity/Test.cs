using System;

namespace CocoriCore.PageLogs
{
    public class Test : EntityBase<Test>
    {
        public string TestName;
        public Type TestType;
        public string TestMethod;
        public object[] Logs;
    }
}