using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocoriCore.PageLogs
{

    public class RunTestCommand : IMessage<RunTestResponse>
    {
    }

    public class RunTestResponse
    {
        public string[] Summary;
    }

    public class TestItem
    {
        public Type Type;
        public string ClassName;
        public string MethodName;
    }

    public class RunTestHandler : MessageHandler<RunTestCommand, RunTestResponse>
    {
        private readonly PageLogsConfiguration configuration;
        private readonly Db databaseBuilder;

        public RunTestHandler(PageLogsConfiguration configuration, Db databaseBuilder)
        {
            this.configuration = configuration;
            this.databaseBuilder = databaseBuilder;
        }

        public override async Task<RunTestResponse> ExecuteAsync(RunTestCommand message)
        {
            await Task.CompletedTask;
            var sb = new List<string>();

            var types = configuration.Assemblies.SelectMany(
                x => x.GetTypes()
                      .Where(t => t.IsAssignableTo<IPageTest>())
                      .SelectMany(t =>
            {
                var methods = t.GetMethods().Where(m => m.GetCustomAttributes(typeof(Xunit.FactAttribute), false).Length > 0).ToArray();
                return methods.Select(m => new TestItem
                {
                    Type = t,
                    ClassName = t.Name,
                    MethodName = m.Name
                }).ToArray();
            }).ToArray()
            ).ToArray();


            foreach (var test in types)
            {
                try
                {
                    var testInstance = (IPageTest)Activator.CreateInstance(test.Type);

                    await Task.Run(() =>
                    {
                        var methodInfo = test.Type.GetMethod(test.MethodName);
                        var result = methodInfo.Invoke(testInstance, null);
                        if (result is Task task)
                        {
                            task.Wait(); ;
                        }
                    });

                    await databaseBuilder.AddTest(test.Type, test.MethodName, testInstance.GetLogs());
                    sb.Add(" OK   " + test.Type.FullName);
                }
                catch (Exception e)
                {
                    sb.Add(" ERROR " + test.Type.FullName + " (" + e.Message + ")");
                }
            }




            return new RunTestResponse()
            {
                Summary = sb.ToArray()
            };
        }
    }
}