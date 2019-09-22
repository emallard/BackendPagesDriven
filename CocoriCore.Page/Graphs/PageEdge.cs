namespace CocoriCore.Page
{
    public class PageEdge
    {
        public PageNode From;
        public PageNode To;
        public bool IsLink;
        public bool IsForm;
        public string Name;
        public bool Visited = false;
    }
}