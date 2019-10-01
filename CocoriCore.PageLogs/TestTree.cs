using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore.PageLogs
{

    public class TestTreeItem
    {
        public Type Type;
        public string ClassName;
        public string TestName;
    }

    public class TestTreeNode
    {
        public string Name;
        public List<TestTreeNode> Children = new List<TestTreeNode>();
        public List<TestTreeItem> Tests = new List<TestTreeItem>();
    }

    public class TestTree
    {
        public TestTreeItem[] Items;
        public TestTreeNode RootNode;
        public void Init(TestTreeItem[] tests)
        {
            Items = tests;
            RootNode = new TestTreeNode() { Name = "" };
            foreach (var t in tests)
                AddTest(t);
        }

        private void AddTest(TestTreeItem test)
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
                    var newNode = new TestTreeNode();
                    newNode.Name = part;
                    currentNode.Children.Add(newNode);
                    currentNode = newNode;
                }
            }

            currentNode.Tests.Add(test);
        }
    }
}