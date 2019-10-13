using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{

    public class NouveauMotDePasseCommand : IMessage<Empty>
    {
        public Guid Token;
        public string MotDePasse;
        public string Confirmation;
    }


    public class NouveauMotDePasseHandler : MessageHandler<NouveauMotDePasseCommand, Empty>
    {
        private readonly IRepository repository;
        private readonly IHashService hashService;

        public NouveauMotDePasseHandler(IRepository repository, IHashService hashService)
        {
            this.repository = repository;
            this.hashService = hashService;
        }

        public async override Task<Empty> ExecuteAsync(NouveauMotDePasseCommand message)
        {
            var token = await repository.LoadAsync<TokenMotDePasseOublie>(message.Token);
            token.Utilise = true;
            await repository.UpdateAsync(token);

            var utilisateur = await repository.LoadAsync<Utilisateur>(x => x.Email, token.Email);
            utilisateur.HashMotDePasse = await this.hashService.HashAsync(message.MotDePasse);
            await repository.UpdateAsync(utilisateur);

            return new Empty();
        }
    }
}