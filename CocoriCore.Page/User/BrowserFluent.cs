using System;
using System.Linq.Expressions;
using CocoriCore;
using CocoriCore.Page;

namespace CocoriCore.Page
{

    public class BrowserFluent : BrowserFluent<int>
    {
        public BrowserFluent(ICurrentUserLogger logs,
            IBrowser browser,
            IFactory factory) :
             base(logs, browser, factory)
        {
        }
    }


    public class BrowserFluent<TPage>
    {
        public readonly ICurrentUserLogger logger;
        public readonly IBrowser browser;
        private readonly IFactory factory;
        public TPage Page;
        public string Id;

        public BrowserFluent(
            ICurrentUserLogger logs,
            IBrowser browser,
            IFactory factory)
        {
            this.logger = logs;
            this.browser = browser;
            this.factory = factory;
        }

        public BrowserFluent<TPage> SetPageAndId(TPage page)
        {
            Page = page;
            return this;
        }

        public BrowserFluent<T> Follow<T>(Expression<Func<TPage, IMessage<T>>> expressionMessage)
        {
            var nextPage = this.browser.Follow(Page, expressionMessage).Result;

            if (browser is TestBrowser)
            {
                var message = expressionMessage.Compile().Invoke(Page);
                this.logger.Log(new LogFollow
                {
                    MemberName = ((MemberExpression)expressionMessage.Body).Member.Name,
                    PageQuery = (IPageQuery)message,
                });
            }

            return factory.Create<BrowserFluent<T>>().SetPageAndId(nextPage);
        }

        public BrowserFluent<T> Display<T>(IMessage<T> message)
        {
            this.logger.Log(new LogDisplay { PageQuery = message });
            var nextPage = this.browser.Display(message).Result;
            return factory.Create<BrowserFluent<T>>().SetPageAndId(nextPage);
        }

        public BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPage, TPageTo> scenario)
        {
            var scenarioId = Guid.NewGuid();
            this.logger.Log(new LogScenarioStart { ScenarioId = scenarioId, Name = scenario.GetType().Name });

            var result = scenario.Play(this);

            this.logger.Log(new LogScenarioEnd { ScenarioId = scenarioId });

            return result;
        }

        /*
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
        }*/

        public TestBrowserFluentSubmitted<TPage, TFormResponse> Submit<TMessage, TFormResponse>(
            Expression<Func<TPage, Form<TMessage, TFormResponse>>> getForm)
            where TMessage : IMessage, new()
        {
            browser.ApplyBindings((IPageBase)this.Page);
            this.logger.Log(new LogSubmit { MemberName = ((MemberExpression)getForm.Body).Member.Name });
            var formResponse = browser.Submit(Page, getForm).Result;

            return factory.Create<TestBrowserFluentSubmitted<TPage, TFormResponse>>().SetResponse(formResponse);
        }

        public BrowserFluent<TPage> Fill<TMember>(Expression<Func<TPage, TMember>> expressionMember, TMember value)
        {
            this.browser.Fill(this.Page, expressionMember, value);
            return this;
        }

        public BrowserFluent<TPage> Fill<TMember>(Expression<Func<TPage, TMember>> expressionMember, Func<TPage, TMember> value)
        {
            this.browser.Fill(this.Page, expressionMember, value(this.Page));
            return this;
        }

        public BrowserFluent<TPage> Assert(Action<TPage> action)
        {
            action(Page);
            return this;
        }

        public BrowserFluent<TPage> Assert<TMember>(Func<TPage, TMember> memberFunc, params Action<TMember>[] actions)
        {
            var member = memberFunc(Page);
            foreach (var a in actions)
                a(member);
            return this;
        }
    }

    public class TestBrowserFluentSubmitted<TPage, TPostResponse>
    {
        private TPostResponse postResponse;
        private readonly IFactory factory;
        private readonly IBrowser browser;
        private readonly ICurrentUserLogger logger;

        public TestBrowserFluentSubmitted(
            IFactory factory,
            IBrowser browser,
            ICurrentUserLogger logger)
        {
            this.factory = factory;
            this.browser = browser;
            this.logger = logger;
        }

        public TestBrowserFluentSubmitted<TPage, TPostResponse> SetResponse(TPostResponse postResponse)
        {
            this.postResponse = postResponse;
            return this;
        }

        public BrowserFluent<T> ThenFollow<T>(Func<TPostResponse, IMessage<T>> getMessage)
        {
            // TODO difference TestBrowser / SeleniumBrowser
            if (browser is TestBrowser)
            {
                var message = getMessage(postResponse);

                logger.Log(new LogSubmitRedirect()
                {
                    PageQuery = (IPageQuery)message,
                });

                var page = browser.SubmitRedirect(message).Result;
                return factory.Create<BrowserFluent<T>>().SetPageAndId(page);
            }
            else
            {
                var page = browser.SubmitRedirect((IMessage<T>)null).Result;
                return factory.Create<BrowserFluent<T>>().SetPageAndId(page);
            }
        }
    }
}
