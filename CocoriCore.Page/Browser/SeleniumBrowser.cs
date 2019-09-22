using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Router;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace CocoriCore.Page
{

    public class SeleniumBrowser : IBrowser//, IDisposable
    {
        private readonly RouteToUrl routeToUrl;
        public IWebDriver driver;
        public SeleniumBrowser(RouteToUrl routeToUrl)
        {
            driver = new FirefoxDriver();
            this.routeToUrl = routeToUrl;
        }
        public async Task<T> Display<T>(IMessage<T> message)
        {
            await Task.CompletedTask;
            var url = "http://localhost:5000" + routeToUrl.ToUrl(message);
            url = url.Replace("/api", "/");
            driver.Navigate().GoToUrl(url);

            // attendre que la page soit chargée
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElements(By.Id("asyncCallsDone")).Count > 0);


            return DeserializePage<T>();
        }

        public async Task<T> Follow<TPage, T>(TPage page, Expression<Func<TPage, IMessage<T>>> expressionMessage)
        {
            await Task.CompletedTask;
            var body = (MemberExpression)expressionMessage.Body;
            var memberInfo = body.Member;

            driver.FindElement(By.Id(memberInfo.Name)).Click();

            // attendre que la page soit chargée
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElements(By.Id("asyncCallsDone")).Count > 0);

            return DeserializePage<T>();
        }

        public async Task<T> SubmitRedirect<T>(IMessage<T> message)
        {
            await Task.CompletedTask;

            // attendre que la page soit chargée
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElements(By.Id("asyncCallsDone")).Count > 0);

            return DeserializePage<T>();
        }

        public T DeserializePage<T>()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var _page = (object)js.ExecuteScript("return _page;");

            JObject jObject = JObject.FromObject(_page);
            var page = jObject.ToObject<T>();
            return page;
        }
        /*
        public void Dispose()
        {
            //driver.Dispose();
        }
        */

        public async Task<TFormResponse> Submit<TPage, TMessage, TFormResponse>(TPage page, Expression<Func<TPage, Form<TMessage, TFormResponse>>> expressionForm, TMessage message) where TMessage : IMessage, new()
        {
            await Task.CompletedTask;

            var body = (MemberExpression)expressionForm.Body;
            var memberInfo = body.Member;
            var formName = memberInfo.Name;

            var messageType = message.GetType();
            var propertiesAndFields = messageType.GetPropertiesAndFields();
            foreach (var x in propertiesAndFields)
            {
                var elt = driver.FindElement(By.CssSelector("#" + formName + " #" + x.Name));
                var valueToSet = x.InvokeGetter(message);
                if (valueToSet != null)
                {
                    elt.SendKeys(valueToSet.ToString());
                    Thread.Sleep(500);
                }
            }

            driver.FindElement(By.CssSelector("#" + formName + " button")).Click();

            return default(TFormResponse);
        }
    }
}