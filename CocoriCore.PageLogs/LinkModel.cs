namespace CocoriCore.PageLogs
{
    public class LinkModel
    {
        public bool IsLinkModel => true;
        public IPageQuery PageQuery;
        public string Text;

        public LinkModel()
        { }
        public LinkModel(IPageQuery pageQuery, string text)
        {
            PageQuery = pageQuery;
            Text = text;
        }
    }
}