using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CocoriCore.Mapper
{

    public class MapperGraphFormatter
    {
        public string Format(MapperGraph graph)
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph G {");
            sb.AppendLine("    rankdir=\"LR\"");
            sb.AppendLine("    node [margin=\"0.50,0.055\"]");

            sb.AppendLine(SubgraphEntities(graph.Nodes.Where(n => n.Type.IsAssignableTo<IEntity>()), "entities"));
            sb.AppendLine(SubgraphEntities(graph.Nodes.Where(n => n.Type.IsAssignableToGeneric(typeof(IView<>))), "views"));
            sb.AppendLine(SubgraphEntities(graph.Nodes.Where(n =>
                n.Type.IsAssignableToGeneric(typeof(ICreate<>))
                || n.Type.IsAssignableToGeneric(typeof(IUpdate<>))), "writes"));

            foreach (var e in graph.Edges)
                sb.AppendLine("    " + StrEdge(e));

            sb.AppendLine(SubGraphInvis());
            sb.AppendLine("}");

            return CmdDot(sb.ToString());
        }

        private string SubgraphEntities(IEnumerable<MapperNode> nodes, string name)
        {
            var entityNodes = nodes
                .Select(n => "       " + StrNode(n) + "\n")
                .ToArray();
            var entityStr = string.Join("", entityNodes);
            return
                  "    subgraph " + name + " {" + "\n"
                + "        rank=same" + "\n"
                + "        node [shape=\"Mrecord\"]" + "\n"
                + "        rank_" + name + " [style=\"invis\"]" + "\n"
                + entityStr
                + "    }";
        }

        private string SubGraphInvis()
        {
            return
            "    subgraph {" + "\n" +
            "        edge [style=\"invis\"]" + "\n" +
            "        rank_writes -> rank_entities -> rank_views" + "\n" +
            "    }" + "\n";
        }

        private string StrNode(MapperNode node)
        {
            var memberInfos = node.Type.GetPropertiesAndFields().Select(mi => mi.Name);
            return node.Type.Name + " [label=\"{{" + node.Type.Name + " | "
                + string.Join(" \\n ", memberInfos)
                + "}}\"]";
        }

        private string StrEdge(MapperEdge e)
        {
            return "  " + e.From.Type.Name + " -> " + e.To.Type.Name;
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