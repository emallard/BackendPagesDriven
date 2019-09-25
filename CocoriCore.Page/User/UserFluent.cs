using System;
using System.Threading.Tasks;

namespace CocoriCore.Page
{

    public class UserFluent
    {
        public string Id;
        private readonly BrowserFluent browserFluent;
        private readonly MailFluent mailFluent;
        private readonly IUserLogger userLogger;
        private readonly SettableClock settableClock;
        private readonly ICurrentUserLogger currentUserLogger;

        public UserFluent(
            IUserLogger userLogger,
            IEmailReader emailReader,
            SettableClock settableClock,
            ICurrentUserLogger currentUserLogger,
            BrowserFluent browserFluent,
            MailFluent mailFluent)
        {
            this.browserFluent = browserFluent;
            this.userLogger = userLogger;
            this.settableClock = settableClock;
            this.currentUserLogger = currentUserLogger;
            this.mailFluent = mailFluent;
        }

        public UserFluent SetId(string id)
        {
            this.Id = id;
            currentUserLogger.SetUserId(id);
            return this;
        }

        public BrowserFluent<T> Display<T>(IMessage<T> message)
        {
            return browserFluent.Display(message);
        }

        public async Task<MailFluentMessage<T>> ReadEmail<T>(string emailAddress)
        {
            return await mailFluent.Read<T>(emailAddress);
        }

        public UserFluent Comment(string comment)
        {
            this.userLogger.Log(new LogComment() { Id = this.Id, Comment = comment });
            return this;
        }

        public UserFluent Wait(TimeSpan duration)
        {
            this.userLogger.Log(new LogWait() { Id = this.Id, Duration = duration.ToString() });
            settableClock.Now = settableClock.Now.Add(duration);
            return this;
        }
    }
}
