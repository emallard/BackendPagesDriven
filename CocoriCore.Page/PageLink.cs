namespace CocoriCore.Page
{
    public class PageLink
    {
        public bool IsPageLink => true;
        public IPageQuery PageQuery;
        public string Text;

        public PageLink()
        { }
        public PageLink(IPageQuery pageQuery, string text)
        {
            PageQuery = pageQuery;
            Text = text;
        }
    }
}