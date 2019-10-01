using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes.Api
{
    public class FavIconMessage : IMessage<string>
    {
    }

    public class FavIconMessageHandler : MessageHandler<FavIconMessage, string>
    {
        public override async Task<string> ExecuteAsync(FavIconMessage message)
        {
            await Task.CompletedTask;
            return "";
        }
    }
}