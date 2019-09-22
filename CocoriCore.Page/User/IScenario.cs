namespace CocoriCore.Page
{
    public interface IScenario<TPageFrom, TPageTo>
    {
        BrowserFluent<TPageTo> Play(BrowserFluent<TPageFrom> browserFluent);
    }
}
