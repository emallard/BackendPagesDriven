using System.Threading.Tasks;

namespace CocoriCore.Page
{
    public interface IEmailSender
    {
        Task Send(IMyMailMessage mailMessage);
    }
}