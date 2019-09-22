using System.Net.Mail;
using System.Threading.Tasks;

namespace CocoriCore.Page
{

    public interface IEmailReader
    {
        Task<MyMailMessage<T>[]> Read<T>(string email);
    }
}