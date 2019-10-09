using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class TestCsFileQuery : IMessage<TestCsFileResponse>
    {
        public string TestName;
    }

    public class TestCsFileResponse
    {
        public bool IsCsFile => true;
        public string Text;
    }

    public class TestCsFileHandler : MessageHandler<TestCsFileQuery, TestCsFileResponse>
    {
        private readonly IRepository repository;

        public TestCsFileHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public override async Task<TestCsFileResponse> ExecuteAsync(TestCsFileQuery message)
        {
            var test = await repository.LoadAsync<Test>(x => x.TestName, message.TestName);
            /*
            var split = message.TestName.Split(".");
            split = split.Take(split.Length - 1).ToArray();
            var path = System.IO.Path.Combine(split);
            path += ".cs";
            var fullPath = System.IO.Path.GetFullPath(path);*/
            var lines = await File.ReadAllLinesAsync(test.FilePath);
            int startI;
            for (startI = test.LineNumber; startI >= 0; --startI)
            {
                if (lines[startI] == "        {")
                    break;
            }
            int endI;
            for (endI = test.LineNumber; endI < lines.Length; ++endI)
            {
                if (lines[endI] == "        }")
                    break;
            }
            var text = string.Join("\n", lines.Skip(startI - 1).Take(endI - startI + 2));
            return new TestCsFileResponse()
            {
                Text = text
            };
        }
    }
}