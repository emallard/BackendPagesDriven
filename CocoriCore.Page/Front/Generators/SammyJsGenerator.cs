using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CocoriCore.Page
{

    public class SammyJsGeneratorOptions
    {
        public string OutputPath;
    }

    public class SammyJsGenerator
    {
        private readonly SammyJsGeneratorOptions options;
        private readonly PageInspector pageInspector;
        private readonly TypescriptFormatter typescriptFormatter;

        public SammyJsGenerator(
            SammyJsGeneratorOptions options,
            PageInspector pageInspector,
            TypescriptFormatter typescriptFormatter)
        {
            this.options = options;
            this.pageInspector = pageInspector;
            this.typescriptFormatter = typescriptFormatter;
        }

        public void Generate()
        {
            // copy cocoricore.page.fetch.ts

            var javascriptDirectory = System.IO.Path.Combine(options.OutputPath, "javascripts");
            if (!Directory.Exists(javascriptDirectory))
                Directory.CreateDirectory(javascriptDirectory);

            File.WriteAllText(
                System.IO.Path.Combine(options.OutputPath, "javascripts", "pages.ts"),
                GeneratePagesTs());

            File.WriteAllText(
                System.IO.Path.Combine(options.OutputPath, "javascripts", "routes.js"),
                GenerateRoutesJs());

            Console.WriteLine("templates :");
            var templates = GenerateTemplates();
            foreach (var t in templates)
            {
                Console.WriteLine("  - " + t.Path);
                var filename = System.IO.Path.Combine(options.OutputPath, t.Path);
                if (!File.Exists(filename))
                {
                    var directory = System.IO.Path.GetDirectoryName(filename);
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    File.WriteAllText(filename, t.Text);
                }
            }
        }

        private string GeneratePagesTs()
        {
            Console.WriteLine("pages.ts");

            var pageTypeInfos = pageInspector.GetPageTypeInfos();

            var formattedPageTypes = pageTypeInfos
                .Select(p => typescriptFormatter.FormatType(p))
                .ToArray();

            Console.WriteLine("pages : \n" + string.Join("", pageTypeInfos.Select(x => $"  - {x.Name} \n")));

            var typeDiscovery = new TypeDiscovery();
            var neededTypes = typeDiscovery.GetNeededTypes(pageInspector.GetPageResponseTypes());

            var formattedNeededTypes = neededTypes
                .Select(p => typescriptFormatter.FormatType(pageInspector.GetTypeInfo(p)))
                .ToArray();

            return string.Join("", formattedNeededTypes)
                + string.Join("", formattedPageTypes);
        }

        private string GenerateRoutesJs()
        {
            Console.WriteLine("routes.js");

            var routesAndNames = pageInspector.GetPageTypeInfos()
                        .Select(x => new
                        {
                            Route = x.PageUrl.Replace("/api", ""),
                            Name = x.Name,
                            TemplateUrl = GetTemplateFilenameWithoutExtension(x) + ".html"
                        })
                        .ToArray();

            Console.WriteLine(string.Join("", routesAndNames.Select(x => $"  - {x.Route} : {x.Name} \n")));

            return
                "function addRoutes(app) {\n"
                + string.Join("",
                    routesAndNames
                        .Select(x =>
                            $"app.addPage(\"{x.Route}\", {x.Name}Component, \"{x.TemplateUrl}\");\n"
                        ))
                + "}";
        }

        private IEnumerable<PathAndText> GenerateTemplates()
        {
            return pageInspector
            .GetPageTypeInfos()
            .SelectMany(x =>
            {
                var path = GetTemplateFilenameWithoutExtension(x);

                return new PathAndText[]{
                    new PathAndText()
                    {
                        Path = path + ".html",
                        Text = ""
                    },
                    new PathAndText()
                    {
                        Path = path + ".ts",
                        Text = $"class {x.Name}Component extends {x.Name} " + "{\n"
+ @"
    constructor() {
        super();
    }

    async postInit() {
    }
}
"
                    }
                };
            })
            .ToArray();
        }

        private string GetTemplateFilenameWithoutExtension(FrontTypeInfo typeInfo)
        {
            if (typeInfo.Name.StartsWith("Users"))
                return System.IO.Path.Combine("templates", "users", typeInfo.Name + "Component");
            if (typeInfo.Name.StartsWith("Vendeur"))
                return System.IO.Path.Combine("templates", "vendeur", typeInfo.Name + "Component");
            return System.IO.Path.Combine("templates", typeInfo.Name + "Component");
        }

    }

}
