using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class PosteCreateDefaultValueQuery : IMessage<PosteCreateDefaultValueResponse>
    {

    }

    public class PosteCreateDefaultValueResponse
    {
        public string Nom;
    }

    public class PosteCreateDefaultValueHandler : MessageHandler<PosteCreateDefaultValueQuery, PosteCreateDefaultValueResponse>
    {
        public async override Task<PosteCreateDefaultValueResponse> ExecuteAsync(PosteCreateDefaultValueQuery message)
        {
            await Task.CompletedTask;
            return new PosteCreateDefaultValueResponse()
            {
                Nom = "Voitures"
            };
        }
    }
}