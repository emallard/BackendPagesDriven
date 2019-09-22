using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CocoriCore.Page
{

    public class NavigoGeneratorOptions
    {
        public string OutputPath;
    }

    public class NavigoGenerator
    {
        private readonly NavigoGeneratorOptions options;
        private readonly PageInspector pageInspector;
        private readonly TypescriptFormatter typescriptFormatter;

        public NavigoGenerator(
            NavigoGeneratorOptions options,
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
            if (!Directory.Exists(componentsDirectory))
                Directory.CreateDirectory(componentsDirectory);

            File.WriteAllText(
                System.IO.Path.Combine(componentsDirectory, "main2.ts"),
                GenerateMain());


        }

        private string GenerateMain()
        {
            return @"
import { LitElement, html, property, customElement } from ""lit-element"";
import Navigo from ""navigo/lib/navigo.es.js"";

@customElement('my-app')
class MyApp extends LitElement {
  @property() route;

  constructor() {
    super()
    let router = new Navigo('/', false)
    
    /*ROUTER*/
    
    router.resolve()
  }
  render() {
    return html`
      <div>
        <h1>MyAwesomeApp</h1>
        ${this.route}
      </div>
    `
  }
}

".Replace("/*ROUTER*/", GetRoutes());
        }

        private string GetRoutes()
        {
            var pageTypes = pageInspector.GetPageTypeInfos();
            return "router\n"
            + string.Join("", pageTypes.Select(x => GetPart(x.PageUrl, GetComponentName(x))))
            + "    ;\n";
        }

        private string GetPart(string pageUrl, string pageTag)
        {
            return $"    .on('{pageUrl}', () => {{ this.route = html`<{pageTag}></{pageTag}>` }})" + "\n";
        }

        private string GetComponentName(FrontTypeInfo typeInfo)
        {
            var name = typeInfo.Name;
            return name.Replace("_", "-").ToLowerInvariant();
        }
    }

}
