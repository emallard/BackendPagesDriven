using System;
using System.Threading.Tasks;

namespace CocoriCore.Page
{

    public class UserFluent
    {
        public string UserName;
        private readonly MailFluent mailFluent;
        private readonly BrowserFluent browserFluent;
        private readonly IUserLogger userLogger;
        private readonly SettableClock settableClock;
        private readonly ICurrentUserLogger currentUserLogger;
        private readonly IFactory factory;

        public UserFluent(
            IUserLogger userLogger,
            IEmailReader emailReader,
            SettableClock settableClock,
            ICurrentUserLogger currentUserLogger,
            IFactory factory,
            MailFluent mailFluent,
            BrowserFluent browserFluent)
        {
            this.userLogger = userLogger;
            this.settableClock = settableClock;
            this.currentUserLogger = currentUserLogger;
            this.factory = factory;
            this.mailFluent = mailFluent;
            this.browserFluent = browserFluent;
        }

        public UserFluent SetUserName(string id)
        {
            this.UserName = id;
            currentUserLogger.SetUserId(id);
            return this;
        }

        public BrowserFluent<T> Display<T>(IMessage<T> message) where T : IPageBase
        {
            return browserFluent.Display(message);
        }
        public BrowserFluent<TPageTo> Play<TPageTo>(IScenario<TPageTo> scenario) where TPageTo : IPageBase
        {
            return browserFluent.Play(scenario);
        }


        public async Task<MailFluentMessage<T>> ReadEmail<T>(string emailAddress)
        {
            return await mailFluent.Read<T>(emailAddress);
        }

        public UserFluent Comment(string comment)
        {
            this.userLogger.Log(new LogComment() { UserName = this.UserName, Comment = comment });
            return this;
        }

        public UserFluent Wait(TimeSpan duration)
        {
            this.userLogger.Log(new LogWait() { UserName = this.UserName, Duration = duration.ToString() });
            settableClock.Now = settableClock.Now.Add(duration);
            return this;
        }
    }
}
