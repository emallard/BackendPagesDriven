using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace MultiUser
{
    public class TokenMotDePasseOublieQuery : IMessage<TokenMotDePasseOublieResponse>
    {
        public ID<TokenMotDePasseOublie> IdToken;
    }

    public class TokenMotDePasseOublieResponse
    {
        public bool Expiré;
        public bool Utilisé;
    }
    public class TokenMotDePasseOublieHandler : MessageHandler<TokenMotDePasseOublieQuery, TokenMotDePasseOublieResponse>
    {
        private readonly IRepository repository;
        private readonly IClock clock;

        public TokenMotDePasseOublieHandler(IRepository repository, IClock clock)
        {
            this.repository = repository;
            this.clock = clock;
        }
        public override async Task<TokenMotDePasseOublieResponse> ExecuteAsync(TokenMotDePasseOublieQuery message)
        {
            var token = await repository.LoadAsync<TokenMotDePasseOublie>(message.IdToken);

            return new TokenMotDePasseOublieResponse()
            {
                Utilisé = token.Utilise,
                Expiré = clock.Now > token.DateExpiration,
            };
        }
    }
}