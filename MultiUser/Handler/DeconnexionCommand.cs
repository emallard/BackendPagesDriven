using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace MultiUser
{

    public class DeconnexionCommand : ICommand, IMessage<DeconnexionResponse>
    {
    }

    public class DeconnexionResponse
    {
        public IClaims Claims;
    }


    public class DeconnexionHandler : MessageHandler<DeconnexionCommand, DeconnexionResponse>
    {
        public override async Task<DeconnexionResponse> ExecuteAsync(DeconnexionCommand message)
        {
            await Task.CompletedTask;
            return new DeconnexionResponse()
            {
                Claims = null
            };
        }
    }
}
