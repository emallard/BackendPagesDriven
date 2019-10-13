using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace MultiUser
{

    public class ConnexionCommand : ICommand, IMessage<ConnexionResponse>
    {
        public string Email;
        public string Password;
    }

    public class ConnexionResponse
    {
        public IClaims Claims;
    }


    public class ConnexionHandler : MessageHandler<ConnexionCommand, ConnexionResponse>
    {
        private readonly IRepository repository;
        private readonly IHashService hashService;

        public ConnexionHandler(IRepository repository, IHashService hashService)
        {
            this.repository = repository;
            this.hashService = hashService;
        }

        public override async Task<ConnexionResponse> ExecuteAsync(ConnexionCommand message)
        {
            var utilisateur = await repository
                .Query<Utilisateur>()
                .Where(x => x.Email == message.Email)
                .FirstOrDefaultAsync();

            if (utilisateur == null)
                throw new Exception("Validation Exception no corresponding user");

            if (!await hashService.PasswordMatchHashAsync(message.Password, utilisateur.HashMotDePasse))
                throw new Exception("Validation Exception no corresponding user");

            return new ConnexionResponse()
            {
                Claims = new UserClaims() { IdUtilisateur = utilisateur.Id }
            };
        }
    }
}
