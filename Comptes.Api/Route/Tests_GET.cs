using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes.Api
{
    public class Tests_GET : IMessage<TestTree>
    {

    }

    public class TestItem
    {
        public Type Type;
        public string ClassName;
        public string TestName;
    }

    public class TestNode
    {
        public string Name;
        public List<TestNode> Children = new List<TestNode>();
        public List<TestItem> Tests = new List<TestItem>();
    }

    public class TestTree
    {
        public TestItem[] Items;
        public TestNode RootNode;
        public void Init(TestItem[] tests)
        {
            Items = tests;
            RootNode = new TestNode() { Name = "" };
            foreach (var t in tests)
                AddTest(t);
        }

        private void AddTest(TestItem test)
        {
            var parts = test.Type.FullName.Split(".");
            var currentNode = RootNode;
            for (var i = 0; i < parts.Length; ++i)
            {
                var part = parts[i];
                var found = currentNode.Children.FirstOrDefault(x => x.Name == part);
                if (found != null)
                    currentNode = found;
                else
                {
                    var newNode = new TestNode();
                    newNode.Name = part;
                    currentNode.Children.Add(newNode);
                    currentNode = newNode;
                }
            }

            currentNode.Tests.Add(test);
        }
    }

    public class Tests_GETHandler : MessageHandler<Tests_GET, TestTree>
    {
        public override async Task<TestTree> ExecuteAsync(Tests_GET message)
        {
            await Task.CompletedTask;
            var assembly = Comptes.AssemblyInfo.Assembly;
            var items = assembly.GetTypes().SelectMany(t =>
            {
                var methods = t.GetMethods().Where(m => m.GetCustomAttributes(typeof(Xunit.FactAttribute), false).Length > 0).ToArray();
                return methods.Select(m => new TestItem
                {
                    Type = t,
                    ClassName = t.Name,
                    TestName = m.Name
                }).ToArray();
            }).ToArray();

            var tree = new TestTree();
            tree.Init(items);
            return tree;
        }
    }
}