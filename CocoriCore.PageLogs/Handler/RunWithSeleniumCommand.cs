using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocoriCore.Router;

namespace CocoriCore.PageLogs
{
    public class RunWithSeleniumCommand : IMessage<Empty>
    {
        public string TestName;
    }

    public class RunWithSeleniumHandler : MessageHandler<RunWithSeleniumCommand, Empty>
    {
        private readonly IRepository repository;
        private readonly RouterOptions routerOptions;

        public RunWithSeleniumHandler(
            IRepository repository,
            RouterOptions routerOptions)
        {
            this.repository = repository;
            this.routerOptions = routerOptions;
        }
        public override async Task<Empty> ExecuteAsync(RunWithSeleniumCommand message)
        {
            var test = await repository.LoadAsync<Test>(x => x.TestName, message.TestName);


            var testInstance = (IPageTest)Activator.CreateInstance(test.TestType);
            testInstance.WithSeleniumBrowser(routerOptions);

            await Task.Run(() =>
            {
                var methodInfo = test.TestType.GetMethod(test.TestMethod);
                var result = methodInfo.Invoke(testInstance, null);
                if (result is Task task)
                {
                    task.RunSynchronously();
                }
            });

            return new Empty();
        }
    }
}