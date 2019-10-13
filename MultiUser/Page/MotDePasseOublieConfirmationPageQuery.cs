using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{
    public class MotDePasseOublieConfirmationPageQuery : IPageQuery<MotDePasseOublieConfirmationPage>
    {
    }

    public class MotDePasseOublieConfirmationPage : PageBase<MotDePasseOublieConfirmationPageQuery>
    {

    }

    public class MotDePasseOublieConfirmationPageModule : PageModule
    {
        public MotDePasseOublieConfirmationPageModule()
        {
            HandlePage<MotDePasseOublieConfirmationPageQuery, MotDePasseOublieConfirmationPage>();
        }
    }

}