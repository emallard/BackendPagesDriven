using System;

namespace CocoriCore.Page
{
    public class LogScenarioStart : UserLog
    {
        public bool IsScenarioStart = true;
        public Guid ScenarioId;
        public string Name;
    }
}
