namespace CocoriCore.Page
{
    public interface IScenario<TPageFrom, TPageTo>
        where TPageTo : IPageBase
        where TPageFrom : IPageBase
    {
        BrowserFluent<TPageTo> Play(BrowserFluent<TPageFrom> browserFluent);
    }
}
