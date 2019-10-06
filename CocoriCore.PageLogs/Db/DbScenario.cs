using System;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbScenario
    {
        public async Task Insert(DbInsertContext context, LogScenarioStart logScenarioStart)
        {
            await Task.CompletedTask;
            context.Scenarios.Add(logScenarioStart);
            context.ScenarioNames = string.Join("/", context.Scenarios);
        }

        public async Task Insert(DbInsertContext context, LogScenarioEnd logScenarioEnd)
        {
            await Task.CompletedTask;
            if (context.Scenarios.Count == 0)
                throw new Exception("Erreur d'algorithmique");
            var lastScenario = context.Scenarios[context.Scenarios.Count - 1];
            if (lastScenario.ScenarioId != logScenarioEnd.ScenarioId)
                throw new Exception("Erreur d'algorithmique");

            context.Scenarios.RemoveAt(context.Scenarios.Count - 1);
            context.ScenarioNames = string.Join("/", context.Scenarios);
        }
    }
}