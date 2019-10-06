using System.Collections.Generic;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbInsertContext
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public List<LogScenarioStart> Scenarios = new List<LogScenarioStart>();
        public string ScenarioNames = "";
        public string PageName;
        public TestPage Page;
        public TestPageRedirection Redirection;
        public string PageMemberName;
        public string MessageName;
    }
}