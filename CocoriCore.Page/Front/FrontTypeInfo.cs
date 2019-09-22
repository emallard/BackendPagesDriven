using System.Collections.Generic;
using System.Reflection;

namespace CocoriCore.Page
{
    public class FrontTypeInfo
    {
        public string Name;
        public FieldInfo[] FieldInfos;
        public bool IsPage;
        public string PageUrl;
        public LinkMemberInfo[] LinkMemberInfos;
        public FormMemberInfo[] FormMemberInfos;
    }
}
