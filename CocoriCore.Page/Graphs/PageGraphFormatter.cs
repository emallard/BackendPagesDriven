using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CocoriCore.Page
{
    public class PageGraphFormatter
    {
        public string LinksAndForms(PageGraph graph)
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph G {");

            foreach (var n in graph.Nodes.OrderBy(x => x.ParameterizedUrl).ToArray())
                sb.AppendLine(StrNode(n));

            foreach (var e in graph.Edges)
                sb.AppendLine(StrEdge(e));

            sb.AppendLine("}");
            return CmdDot(sb.ToString());
        }

        public string LinksAndFormsWithClusters(PageGraph graph)
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph G {");

            var clusters = graph.Nodes
                            .Where(x => x.ParameterizedUrl != "/api")
                            .GroupBy(x => x.ParameterizedUrl.Split("/", StringSplitOptions.RemoveEmptyEntries)[1])
                            .ToArray();


            sb.AppendLine(StrNode(graph.Nodes.First(n => n.ParameterizedUrl == "/api")));

            foreach (var g in clusters)
            {
                sb.AppendLine($"subgraph cluster_{g.Key}" + "{");
                sb.AppendLine("style=filled;");
                sb.AppendLine("color=lightgrey;");
                sb.AppendLine($"label=\"{g.Key}\";");
                foreach (var n in g)
                    sb.AppendLine(StrNode(n));
                sb.AppendLine("}");
            }


            foreach (var e in graph.Edges)
                sb.AppendLine(StrEdge(e));

            sb.AppendLine("}");
            return CmdDot(sb.ToString());
        }

        private string StrNode(PageNode n)
        {
            var label = n.ParameterizedUrl.Replace("/api", "");
            if (label == "")
                label = "/";
            return $"{n.IndexedName}[label=\"{label}\"];";
        }

        private string StrEdge(PageEdge e)
        {
            var sb = new StringBuilder();
            sb.Append($"{e.From.IndexedName} -> {e.To.IndexedName}");
            var properties = new List<string>();
            properties.Add($"tooltip=\"{e.Name}\"");
            if (e.Visited)
                properties.Add("color=\"#FF0000\"");
            if (e.IsForm)
            {
                properties.Add("arrowhead=empty");
                properties.Add($"label=\"{e.Name}\"");
                if (!e.Visited)
                    properties.Add("color=\"#0000FF\"");
            }
            if (properties.Any())
                sb.Append("[" + string.Join(",", properties) + "]");

            sb.Append(";");
            return sb.ToString();
        }

        private string CmdDot(string content)
        {
            var tmp = System.IO.Path.GetTempFileName();
            Console.WriteLine(tmp);
            File.WriteAllText(tmp, content);
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = "-Tsvg " + tmp,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            if (error.Length > 0)
                throw new Exception(error);
            process.WaitForExit();

            return output;
        }
    }
}