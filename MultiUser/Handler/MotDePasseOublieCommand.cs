using System;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Page;

namespace MultiUser
{

    public class MotDePasseOublieCommand : IMessage<Empty>, ICommand
    {
        public string Email;
    }


    public class MotDePasseOublieHandler : MessageHandler<MotDePasseOublieCommand, Empty>
    {
        private readonly IEmailSender emailSender;
        private readonly IRepository repository;
        private readonly IClock clock;

        public MotDePasseOublieHandler(
            IEmailSender emailSender,
            IRepository repository,
            IClock clock)
        {
            this.emailSender = emailSender;
            this.repository = repository;
            this.clock = clock;
        }
        public async override Task<Empty> ExecuteAsync(MotDePasseOublieCommand message)
        {
            var token = new TokenMotDePasseOublie()
            {
                Email = message.Email,
                Utilise = false,
                DateExpiration = clock.Now.AddHours(1)
            };
            await repository.InsertAsync(token);


            await this.emailSender.Send(new MyMailMessage<EmailMotDePasseOublie>()
            {
                From = "from@example.com",
                To = "aa@aa.aa",
                Body = new EmailMotDePasseOublie()
                {
                    Lien = new SaisieNouveauMotDePassePageQuery()
                    {
                        IdToken = token.Id
                    }
                }
            });
            return new Empty();
        }
    }
}