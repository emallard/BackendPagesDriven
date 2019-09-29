using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Test;

namespace CocoriCore.PageLogs
{
    public class TestListQuery : IMessage<string[]>
    {
    }
    public class TestListHandler : MessageHandler<TestListQuery, string[]>
    {
        private readonly IRepository repository;

        public TestListHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public override async Task<string[]> ExecuteAsync(TestListQuery message)
        {
            return (await repository.Query<Test>()
                .Select(x => x.TestName).ToArrayAsync()).Distinct().ToArray();
        }
    }
}