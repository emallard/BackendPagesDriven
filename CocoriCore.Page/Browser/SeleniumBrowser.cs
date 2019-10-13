using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Router;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace CocoriCore.Page
{

    public class SeleniumBrowser : IBrowser//, IDisposable
    {
        private readonly JsonSerializer jsonSerializer;
        private readonly RouteToUrl routeToUrl;
        public IWebDriver driver;
        public SeleniumBrowser(
            JsonSerializer jsonSerializer,
            RouteToUrl routeToUrl)
        {
            driver = new FirefoxDriver();
            this.jsonSerializer = jsonSerializer;
            this.routeToUrl = routeToUrl;
            Thread.Sleep(3000);
        }
        public async Task<T> Display<T>(IMessage<T> message) where T : IPageBase
        {
            await Task.CompletedTask;
            var url = "http://localhost:5000" + routeToUrl.ToUrl(message);
            url = url.Replace("/api", "/");
            driver.Navigate().GoToUrl(url);

            // attendre que la page soit chargée
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(d => d.FindElements(By.Id("asyncCallsDone")).Count > 0);
            Thread.Sleep(1000);

            return DeserializePage<T>();
        }

        public async Task<T> Follow<TPage, T>(TPage page, Expression<Func<TPage, IMessage<T>>> expressionMessage)
            where T : IPageBase
        {
            await Task.CompletedTask;
            var idMessage = ExprToString(expressionMessage);
            driver.FindElement(By.Id(idMessage)).Click();

            // attendre que la page soit chargée
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(d => d.FindElements(By.Id("asyncCallsDone")).Count > 0);
            Thread.Sleep(500);

            return DeserializePage<T>();
        }

        public async Task<T> SubmitRedirect<T>(IMessage<T> message) where T : IPageBase
        {
            await Task.CompletedTask;

            // attendre que la page soit chargée
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(d => d.FindElements(By.Id("asyncCallsDone")).Count > 0);
            Thread.Sleep(1000);

            return DeserializePage<T>();
        }

        public T DeserializePage<T>()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var _page = (object)js.ExecuteScript("return _page;");

            JObject jObject = JObject.FromObject(_page);
            var page = jObject.ToObject<T>(this.jsonSerializer);
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

            var formId = ExprToString(expressionForm);

            var messageType = message.GetType();
            var propertiesAndFields = messageType.GetPropertiesAndFields();
            foreach (var x in propertiesAndFields)
            {
                var elt = driver.FindElement(By.Id(formId + "." + x.Name));
                var valueToSet = x.InvokeGetter(message);
                if (valueToSet != null)
                {
                    elt.Clear();
                    elt.SendKeys(valueToSet.ToString());
                    Thread.Sleep(500);
                }
            }

            var form = driver.FindElement(By.Id(formId));
            var button = form.FindElement(By.CssSelector("button"));
            button.Click();

            return default(TFormResponse);
        }

        public async Task<TFormResponse> Submit<TPage, TMessage, TFormResponse>(TPage page, Expression<Func<TPage, Form<TMessage, TFormResponse>>> expressionForm)
            where TPage : IPageBase
            where TMessage : IMessage, new()
        {
            await Task.CompletedTask;

            var elt = driver.FindElement(By.Id(ExprToString(expressionForm)));
            var button = elt.FindElement(By.CssSelector("button"));
            button.Click();

            return default(TFormResponse);
        }

        public void Fill<TPage, TMember>(TPage page, Expression<Func<TPage, TMember>> expressionMember, TMember value)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var id = ExprToString(expressionMember.Body);
            var jsObject = jsonSerializer.Serialize(value);
            js.ExecuteScript("_page." + id + " = " + jsObject);
            js.ExecuteScript("onPageUpdate('" + id + "', " + jsObject + ");");
        }


        private string ExprToString(Expression expr)
        {
            var expression = expr;
            var memberInfos = new List<MemberInfo>();

            if (expression.NodeType == ExpressionType.Convert)
                expression = ((UnaryExpression)expression).Operand;

            var str = expression.ToString();
            return str.Substring(str.IndexOf(".") + 1);
        }

        public async Task<TFormResponse> Click<TPage, TMessage, TFormResponse>(TPage page, Expression<Func<TPage, ActionCall<TMessage, TFormResponse>>> getCall)
            where TPage : IPageBase
            where TMessage : IMessage, new()
        {
            await Task.CompletedTask;

            var button = driver.FindElement(By.Id(ExprToString(getCall)));
            button.Click();

            return default(TFormResponse);
        }
    }
}