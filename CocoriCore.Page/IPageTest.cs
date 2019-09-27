using CocoriCore.Router;

namespace CocoriCore
{
    public interface IPageTest
    {
        void WithSeleniumBrowser(RouterOptions routerOptions);
        object[] GetLogs();
    }
}