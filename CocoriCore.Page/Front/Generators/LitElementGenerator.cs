using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CocoriCore.Page
{

    public class LitElementGeneratorOptions
    {
        public string OutputPath;
    }

    public class LitElementGenerator
    {
        private readonly LitElementGeneratorOptions options;
        private readonly PageInspector pageInspector;
        private readonly TypescriptFormatter typescriptFormatter;

        public LitElementGenerator(
            LitElementGeneratorOptions options,
            PageInspector pageInspector,
            TypescriptFormatter typescriptFormatter)
        {
            this.options = options;
            this.pageInspector = pageInspector;
            this.typescriptFormatter = typescriptFormatter;
        }

        public void Generate()
        {
            var componentsDirectory = System.IO.Path.Combine(options.OutputPath);
            if (Directory.Exists(componentsDirectory))
                Directory.Delete(componentsDirectory, true);
            if (!Directory.Exists(componentsDirectory))
                Directory.CreateDirectory(componentsDirectory);

            File.WriteAllText(
                System.IO.Path.Combine(componentsDirectory, "initPage.ts"),
                GenerateInitPages());

            File.WriteAllText(
                System.IO.Path.Combine(componentsDirectory, "types.ts"),
                GenerateTypesTs());

            Console.WriteLine("components :");
            var templates = GenerateComponents();
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

        private string GenerateTypesTs()
        {
            Console.WriteLine("types.ts");

            var pageTypeInfos = pageInspector.GetPageTypeInfos();

            /*
            var formattedPageTypes = pageTypeInfos
                .Select(p => typescriptFormatter.FormatType(p))
                .ToArray();

            Console.WriteLine("pages : \n" + string.Join("", pageTypeInfos.Select(x => $"  - {x.Name} \n")));
            */

            var typeDiscovery = new TypeDiscovery();
            var neededTypes = typeDiscovery.GetNeededTypes(pageInspector.GetPageResponseTypes());

            var formattedNeededTypes = neededTypes
                .Select(p => typescriptFormatter.FormatType(pageInspector.GetTypeInfo(p)))
                .ToArray();

            return string.Join("", formattedNeededTypes);
            //+ string.Join("", formattedPageTypes);
        }

        private IEnumerable<PathAndText> GenerateComponents()
        {
            return pageInspector
            .GetPageTypeInfos()
            .SelectMany(x =>
            {
                var path = GetTemplateFilenameWithoutExtension(x);

                return new PathAndText[]{
                    new PathAndText()
                    {
                        Path = path + ".ts",
                        Text = FormatPage(x)
                    }
                };
            })
            .ToArray();
        }

        private string FormatPage(FrontTypeInfo typeInfo)
        {
            var componentName = GetComponentName(typeInfo);

            var header = @"import { LitElement, html, property, customElement } from ""lit-element""" + "\n"
            + "import { InitPage } from './initPage';\n"
            + $"@customElement('{componentName}')" + "\n"
            + $"class {typeInfo.Name}Component extends LitElement" + "{\n";

            var footer = "}\n";

            var props =
                string.Join("",
                    typeInfo.FieldInfos.Select(x =>
                    "@property({type : " + GetPropertyType(x.FieldType) + "}) " + $"{x.Name}: {ConvertTypeToTypeScript(x.FieldType)}" + "\n")
                );

            var connected = "connectedCallback() { \n"
                          + "    super.connectedCallback()\n"
                         + $"    new InitPage().Fill('/api/' + location.pathname, this);\n"
                          + "}\n";

            var render = "render() { return html`" + componentName + "`; }";

            return header + props /*+ connected*/ + render + footer;
        }

        private string GetComponentName(FrontTypeInfo typeInfo)
        {
            var name = typeInfo.Name;
            return name.Replace("_", "-").ToLowerInvariant();
        }

        private string GetPropertyType(Type t)
        {
            if (t.IsArray)
                return "Array";
            if (t == typeof(string))
                return "String";
            return "Object";
        }

        private string ConvertTypeToTypeScript(Type t)
        {
            if (t.IsArray)
                return ConvertTypeToTypeScript(t.GetElementType()) + "[]";
            if (t == typeof(string))
                return "string";
            return "t.Name";
        }

        private string GetTemplateFilenameWithoutExtension(FrontTypeInfo typeInfo)
        {
            /*
            if (typeInfo.Name.StartsWith("Users"))
                return System.IO.Path.Combine("users", typeInfo.Name + "Component");
            if (typeInfo.Name.StartsWith("Vendeur"))
                return System.IO.Path.Combine("vendeur", typeInfo.Name + "Component");
            */
            return System.IO.Path.Combine(typeInfo.Name + "Component");
        }

        private string GenerateInitPages()
        {
            return @"
export class InitPage {
    async Fill(url: string, obj: object): Promise<void> {
        console.log('fetch page data : ' + url);
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        let response = await fetch(url,
            {
                headers: myHeaders,
                method: 'GET'
            });

        let txt = await response.text();

        console.log('page data as text : ' + txt);

        if (txt.length > 0) {
            var obj = JSON.parse(txt);
            var keys = Object.keys(obj);
            for (let k of keys) {
                console.log('page property : ' + k);
                this[k] = obj[k];
            }
        }
    }
}";
        }

    }

}
