namespace CocoriCore.Page
{
    public interface IScenario<TPageFrom, TPageTo>
        where TPageTo : IPageBase
        where TPageFrom : IPageBase
    {
        BrowserFluent<TPageTo> Play(BrowserFluent<TPageFrom> browserFluent);
    }


    public interface IScenario<TPageTo>
        where TPageTo : IPageBase
    {
        BrowserFluent<TPageTo> Play(IBrowserFluent browserFluent);
    }
}
