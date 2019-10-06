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


    public class PageLink<T> where T : IPageQuery
    {
        public bool IsPageLink => true;
        public T PageQuery;
        public string Text;

        public PageLink()
        { }
        public PageLink(T pageQuery, string text)
        {
            PageQuery = pageQuery;
            Text = text;
        }
    }
}