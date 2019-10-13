using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{
    public class ProfileQuery : IMessage<ProfileResponse>
    {
    }

    public class ProfileResponse
    {
        public string Nom;
        public string Prenom;
    }

    public class ProfileHandler : MessageHandler<ProfileQuery, ProfileResponse>
    {
        private readonly IClaimsProvider claimsProvider;
        private readonly IRepository repository;

        public ProfileHandler(IClaimsProvider claimsProvider, IRepository repository)
        {
            this.claimsProvider = claimsProvider;
            this.repository = repository;
        }

        public override async Task<ProfileResponse> ExecuteAsync(ProfileQuery query)
        {
            var idUtilisateur = claimsProvider.GetClaims<UserClaims>().IdUtilisateur;
            var profile = await repository.LoadAsync<Profile>(x => x.IdUtilisateur, idUtilisateur);

            return new ProfileResponse()
            {
                Nom = profile.Nom,
                Prenom = profile.Prenom
            };
        }
    }
}