﻿using System;
using System.Linq.Expressions;
using CocoriCore;
using CocoriCore.Page;

namespace CocoriCore.Page
{
    public interface IBrowserFluent
    {
        BrowserFluent<T> Display<T>(IMessage<T> message) where T : IPageBase;
        BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPageTo> scenario) where TPageTo : IPageBase;
    }


    public class BrowserFluent : IBrowserFluent
    {
        private readonly ICurrentUserLogger logger;
        private readonly IBrowser browser;
        private readonly IFactory factory;

        public BrowserFluent(
            ICurrentUserLogger logger,
            IBrowser browser,
            IFactory factory)
        {
            this.logger = logger;
            this.browser = browser;
            this.factory = factory;
        }

        public BrowserFluent<T> Display<T>(IMessage<T> message) where T : IPageBase
        {
            this.logger.Log(new LogDisplay { PageQuery = message });
            var nextPage = this.browser.Display(message).Result;
            return factory.Create<BrowserFluent<T>>().SetPageAndId(nextPage);
        }

        public BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPageTo> scenario)
           where TPageTo : IPageBase
        {
            var scenarioId = Guid.NewGuid();
            this.logger.Log(new LogScenarioStart { ScenarioId = scenarioId, Name = scenario.GetType().Name });
            var result = scenario.Play(this);
            this.logger.Log(new LogScenarioEnd { ScenarioId = scenarioId });
            return result;
        }
    }


    public class BrowserFluent<TPage> : IBrowserFluent where TPage : IPageBase
    {
        public readonly ICurrentUserLogger logger;
        public readonly IBrowser browser;
        private readonly IFactory factory;
        public TPage Page;

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

        public BrowserFluent<T> Follow<T>(Expression<Func<TPage, IMessage<T>>> expressionMessage) where T : IPageBase
        {
            if (browser is TestBrowser)
            {
                var message = expressionMessage.Compile().Invoke(Page);
                this.logger.Log(new LogFollow
                {
                    MemberName = ((MemberExpression)expressionMessage.Body).Member.Name,
                    PageQuery = (IPageQuery)message,
                });
            }
            var nextPage = this.browser.Follow(Page, expressionMessage).Result;
            return factory.Create<BrowserFluent<T>>().SetPageAndId(nextPage);
        }

        public BrowserFluent<T> Display<T>(IMessage<T> message) where T : IPageBase
        {
            this.logger.Log(new LogDisplay { PageQuery = message });
            var nextPage = this.browser.Display(message).Result;
            return factory.Create<BrowserFluent<T>>().SetPageAndId(nextPage);
        }

        public BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPage, TPageTo> scenario)
            where TPageTo : IPageBase
        {
            var scenarioId = Guid.NewGuid();
            this.logger.Log(new LogScenarioStart { ScenarioId = scenarioId, Name = scenario.GetType().Name });
            var result = scenario.Play(this);
            this.logger.Log(new LogScenarioEnd { ScenarioId = scenarioId });
            return result;
        }

        public BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPageTo> scenario)
            where TPageTo : IPageBase
        {
            var scenarioId = Guid.NewGuid();
            this.logger.Log(new LogScenarioStart { ScenarioId = scenarioId, Name = scenario.GetType().Name });
            var result = scenario.Play(this);
            this.logger.Log(new LogScenarioEnd { ScenarioId = scenarioId });
            return result;
        }



        public BrowserFluentSubmitted<TPage, TFormResponse> Submit<TMessage, TFormResponse>(
            Expression<Func<TPage, Form<TMessage, TFormResponse>>> getForm)
            where TMessage : IMessage, new()
        {
            //this.logger.Log(new LogSubmit { MemberName = ((MemberExpression)getForm.Body).Member.Name });
            var formResponse = browser.Submit(Page, getForm).Result;

            return factory.Create<BrowserFluentSubmitted<TPage, TFormResponse>>().SetResponse(formResponse);
        }


        public BrowserFluent<TPage> SubmitShouldFail<TMessage, TFormResponse>(
            Expression<Func<TPage, Form<TMessage, TFormResponse>>> getForm)
            where TMessage : IMessage, new()
        {
            bool exceptionCaught = false;
            try
            {
                var formResponse = browser.Submit(Page, getForm).Result;
            }
            catch (Exception)
            {
                exceptionCaught = true;
            }
            if (!exceptionCaught)
                throw new Exception("Submit should have failed");

            return this;
        }




        public BrowserFluentSubmitted<TPage, TFormResponse> Click<TMessage, TFormResponse>(
            Expression<Func<TPage, ActionCall<TMessage, TFormResponse>>> getCall)
            where TMessage : IMessage, new()
        {
            //this.logger.Log(new LogSubmit { MemberName = ((MemberExpression)getForm.Body).Member.Name });
            var formResponse = browser.Click(Page, getCall).Result;

            return factory.Create<BrowserFluentSubmitted<TPage, TFormResponse>>().SetResponse(formResponse);
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
            this.logger.Log(new LogAssert());
            action(Page);
            return this;
        }

        public BrowserFluent<TPage> Assert<TMember>(Func<TPage, TMember> memberFunc, params Action<TMember>[] actions)
        {
            this.logger.Log(new LogAssert());
            var member = memberFunc(Page);
            foreach (var a in actions)
                a(member);
            return this;
        }
    }

    public class BrowserFluentSubmitted<TPage, TPostResponse>
    {
        private TPostResponse postResponse;
        private readonly IFactory factory;
        private readonly IBrowser browser;
        private readonly ICurrentUserLogger logger;

        public BrowserFluentSubmitted(
            IFactory factory,
            IBrowser browser,
            ICurrentUserLogger logger)
        {
            this.factory = factory;
            this.browser = browser;
            this.logger = logger;
        }

        public BrowserFluentSubmitted<TPage, TPostResponse> SetResponse(TPostResponse postResponse)
        {
            this.postResponse = postResponse;
            return this;
        }

        public BrowserFluent<T> ThenFollow<T>(Func<TPostResponse, IMessage<T>> getMessage) where T : IPageBase
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
