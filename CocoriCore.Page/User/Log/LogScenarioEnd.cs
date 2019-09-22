using System;

namespace CocoriCore.Page
{
    public class LogScenarioEnd : UserLog
    {
        public bool IsScenarioEnd = true;
        public Guid ScenarioId;
    }
}
