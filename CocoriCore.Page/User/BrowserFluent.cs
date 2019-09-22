using System;
using System.Linq.Expressions;
using CocoriCore;
using CocoriCore.Page;

namespace CocoriCore.Page
{

    public class BrowserFluent : BrowserFluent<int>
    {
        public BrowserFluent(IUserLogger logs, IBrowser browser) : base(logs, browser)
        {
        }
    }


    public class BrowserFluent<TPage>
    {
        public readonly IUserLogger logs;
        public readonly IBrowser browser;
        public TPage Page;
        public string Id;

        public BrowserFluent(IUserLogger logs, IBrowser browser)
        {
            this.logs = logs;
            this.browser = browser;
        }

        public BrowserFluent<TPage> SetId(string id)
        {
            Id = id;
            return this;
        }

        public BrowserFluent<TPage> SetPageAndId(TPage page, string id)
        {
            Page = page;
            Id = id;
            return this;
        }

        public BrowserFluent<T> Follow<T>(Expression<Func<TPage, IMessage<T>>> expressionMessage)
        {
            if (browser is TestBrowser)
            {
                var message = expressionMessage.Compile().Invoke(Page);
                this.logs.Log(new LogFollow
                {
                    Id = this.Id,
                    MemberName = ((MemberExpression)expressionMessage.Body).Member.Name,
                    PageQuery = (IPageQuery)message
                });
            }

            var nextPage = this.browser.Follow(Page, expressionMessage).Result;
            return new BrowserFluent<T>(logs, browser).SetPageAndId(nextPage, Id);
        }

        public BrowserFluent<T> Display<T>(IMessage<T> message)
        {
            this.logs.Log(new LogDisplay { Id = this.Id, PageQuery = message });

            var nextPage = this.browser.Display(message).Result;
            return new BrowserFluent<T>(logs, browser).SetPageAndId(nextPage, Id);
        }

        public BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPage, TPageTo> scenario)
        {
            var scenarioId = Guid.NewGuid();
            this.logs.Log(new LogScenarioStart { Id = this.Id, ScenarioId = scenarioId, Name = scenario.GetType().Name });

            var result = scenario.Play(this);

            this.logs.Log(new LogScenarioEnd { Id = this.Id, ScenarioId = scenarioId });

            return result;
        }

        public TestBrowserFluentSubmitted<TPage, TFormResponse> Submit<TMessage, TFormResponse>(
            Expression<Func<TPage, Form<TMessage, TFormResponse>>> getForm,
            Action<TMessage> modifyMessage
        )
            where TMessage : IMessage, new()
        {
            var command = new TMessage();
            modifyMessage(command);

            this.logs.Log(new LogSubmit { Id = this.Id, MemberName = ((MemberExpression)getForm.Body).Member.Name });

            var formResponse = browser.Submit(Page, getForm, command).Result;
            return new TestBrowserFluentSubmitted<TPage, TFormResponse>(this, formResponse);
        }
    }

    public class TestBrowserFluentSubmitted<TPage, TPostResponse>
    {
        private readonly BrowserFluent<TPage> browserFluent;
        private readonly TPostResponse postResponse;

        public TestBrowserFluentSubmitted(
            BrowserFluent<TPage> browserFluent,
            TPostResponse postResponse)
        {
            this.browserFluent = browserFluent;
            this.postResponse = postResponse;
        }

        public BrowserFluent<T> ThenFollow<T>(Func<TPostResponse, IMessage<T>> getMessage)
        {
            // TODO difference TestBrowser / SeleniumBrowser
            if (browserFluent.browser is TestBrowser)
            {
                var message = getMessage(postResponse);
                browserFluent.logs.Log(new LogSubmitRedirect()
                {
                    Id = this.browserFluent.Id,
                    PageQuery = (IPageQuery)message
                });

                var page = browserFluent.browser.SubmitRedirect(message).Result;
                return new BrowserFluent<T>(this.browserFluent.logs, this.browserFluent.browser).SetPageAndId(page, this.browserFluent.Id);
            }
            else
            {
                var page = browserFluent.browser.SubmitRedirect((IMessage<T>)null).Result;
                return new BrowserFluent<T>(this.browserFluent.logs, this.browserFluent.browser).SetPageAndId(page, this.browserFluent.Id);
            }
        }
    }
}
