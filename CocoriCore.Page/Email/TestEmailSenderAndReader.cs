using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocoriCore.Page
{

    public class TestEmailSenderAndReader : IEmailReader, IEmailSender
    {
        public List<IMyMailMessage> ReadMessages = new List<IMyMailMessage>();
        public List<IMyMailMessage> NewMessages = new List<IMyMailMessage>();

        public async Task<MyMailMessage<T>[]> Read<T>(string email)
        {
            await Task.CompletedTask;
            var messages = NewMessages.OfType<MyMailMessage<T>>().Where(x => x.To == email).ToArray();
            ReadMessages.AddRange(messages);
            NewMessages = NewMessages.Where(x => !messages.Contains(x)).ToList();
            return messages;
        }

        public async Task Send(IMyMailMessage mailMessage)
        {
            await Task.CompletedTask;
            NewMessages.Add(mailMessage);
        }
    }
}