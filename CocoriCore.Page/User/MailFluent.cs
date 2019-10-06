using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CocoriCore.Page
{
    public class MailFluent
    {
        private readonly Lazy<UserFluent> userFluent;
        private readonly IEmailReader emailReader;
        private readonly IFactory factory;

        public MailFluent(
            Lazy<UserFluent> userFluent,
            IEmailReader emailReader,
            IFactory factory)
        {
            this.userFluent = userFluent;
            this.emailReader = emailReader;
            this.factory = factory;
        }

        public async Task<MailFluentMessage<T>> Read<T>(string emailAddress)
        {
            var mailMessage = (await this.emailReader.Read<T>(emailAddress)).First();
            return factory.Create<MailFluentMessage<T>>().SetMessage(mailMessage);
        }
    }


    public class MailFluentMessage<TMail>
    {
        private readonly Lazy<UserFluent> userFluent;
        public MyMailMessage<TMail> MailMessage;
        public MailFluentMessage(
            Lazy<UserFluent> userFluent)
        {
            this.userFluent = userFluent;
        }

        public MailFluentMessage<TMail> SetMessage(MyMailMessage<TMail> mailMessage)
        {
            this.MailMessage = mailMessage;
            return this;
        }

        public BrowserFluent<TMessage> Follow<TMessage>(Expression<Func<TMail, IMessage<TMessage>>> expressionLink)
            where TMessage : IPageBase
        {
            var message = (IMessage<TMessage>)expressionLink.Compile().Invoke(this.MailMessage.Body);
            return userFluent.Value.Display(message);
        }
    }
}
