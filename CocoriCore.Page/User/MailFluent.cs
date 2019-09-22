using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CocoriCore.Page
{
    public class MailFluent
    {
        private string id;
        private readonly UserFluent userFluent;
        private readonly IUserLogger userLogger;
        private readonly IEmailReader emailReader;

        public MailFluent(
            UserFluent userFluent,
            IUserLogger userLogger,
            IEmailReader emailReader)
        {
            this.userFluent = userFluent;
            this.userLogger = userLogger;
            this.emailReader = emailReader;
        }

        public MailFluent SetId(string id)
        {
            this.id = id;
            return this;
        }

        public async Task<MailFluentMessage<T>> Read<T>(string emailAddress)
        {
            this.userLogger.Log(new LogReadEmail() { Id = this.id });
            var mailMessage = (await this.emailReader.Read<T>(emailAddress)).First();
            return new MailFluentMessage<T>(
                this.userFluent,
                this.userLogger,
                mailMessage
            ).SetId(this.id);
        }
    }


    public class MailFluentMessage<TMail>
    {
        private readonly UserFluent userFluent;
        private readonly IUserLogger userLogger;
        public readonly MyMailMessage<TMail> MailMessage;

        private string id;
        public MailFluentMessage(
            UserFluent userFluent,
            IUserLogger userLogger,
            MyMailMessage<TMail> mailMessage)
        {
            this.userFluent = userFluent;
            this.userLogger = userLogger;
            this.MailMessage = mailMessage;
        }

        public MailFluentMessage<TMail> SetId(string id)
        {
            this.id = id;
            return this;
        }

        public BrowserFluent<TMessage> Follow<TMessage>(Expression<Func<TMail, IMessage<TMessage>>> expressionLink)
        {
            var message = (IMessage<TMessage>)expressionLink.Compile().Invoke(this.MailMessage.Body);
            return userFluent.Display(message);
        }
    }
}
