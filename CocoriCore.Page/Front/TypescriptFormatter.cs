using System;

namespace CocoriCore.Page
{
    public class TypescriptFormatter
    {

        public string FormatType(FrontTypeInfo type)
        {
            return type.IsPage ? FormatPage(type) : FormatMessage(type);
        }

        private string FormatMessage(FrontTypeInfo type)
        {
            var r = $"class {type.Name} " + "{\n";

            foreach (var m in type.FieldInfos)
                r += $"    {m.Name}: {m.FieldType.Name};\n";

            foreach (var m in type.LinkMemberInfos)
                r += $"    {m.Name}: string;\n";

            r += "}\n\n";
            return r;
        }

        private string FormatPage(FrontTypeInfo type)
        {
            var r = $"abstract class {type.Name} extends Page " + "{\n";

            r += $"    PageUrl:string = '{type.PageUrl}';\n";

            foreach (var m in type.FieldInfos)
                r += $"    {m.Name}: {m.FieldType.Name};\n";

            foreach (var m in type.LinkMemberInfos)
                r += $"    {m.Name}: string;\n";

            foreach (var m in type.FormMemberInfos)
                r += $"    {m.Name}: Form<{m.MessageType.Name}, {m.ResponseType.Name}>;\n";

            r += "}\n\n";
            return r;
        }



    }
}
