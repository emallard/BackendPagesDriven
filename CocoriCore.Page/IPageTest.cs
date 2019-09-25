namespace CocoriCore
{
    public interface IPageTest
    {
        void WithSeleniumBrowser();
        object[] GetLogs();
    }
}