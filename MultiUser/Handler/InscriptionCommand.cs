using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{

    public class InscriptionCommand : ICommand, IMessage<InscriptionResponse>
    {
        public string Email;
        public string Password;
        public string PasswordConfirmation;
        public string Nom;
        public string Prenom;

    }

    public class InscriptionResponse
    {
        public IClaims Claims;
    }

    public class InscriptionHandler : MessageHandler<InscriptionCommand, InscriptionResponse>
    {
        private readonly IRepository repository;
        private readonly IHashService hashService;

        public InscriptionHandler(IRepository repository, IHashService hashService)
        {
            this.repository = repository;
            this.hashService = hashService;
        }

        public override async Task<InscriptionResponse> ExecuteAsync(InscriptionCommand message)
        {
            var utilisateur = new Utilisateur()
            {
                Email = message.Email,
                HashMotDePasse = await this.hashService.HashAsync(message.Password)
            };
            await repository.InsertAsync(utilisateur);

            var profile = new Profile()
            {
                IdUtilisateur = utilisateur.Id,
                Nom = message.Nom,
                Prenom = message.Prenom
            };

            await repository.InsertAsync(profile);

            return new InscriptionResponse()
            {
                Claims = new UserClaims() { IdUtilisateur = utilisateur.Id }
            };
        }
    }
}