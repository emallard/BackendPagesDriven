using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class PosteCreateInitQuery : IMessage<PosteCreateInitResponse>
    {

    }

    public class PosteCreateInitResponse
    {
        public string Nom;
    }

    public class PosteCreateInitHandler : MessageHandler<PosteCreateInitQuery, PosteCreateInitResponse>
    {
        public async override Task<PosteCreateInitResponse> ExecuteAsync(PosteCreateInitQuery message)
        {
            await Task.CompletedTask;
            return new PosteCreateInitResponse()
            {
                Nom = "Voiture"
            };
        }
    }
}