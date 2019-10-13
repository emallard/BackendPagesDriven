using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{
    public class MotDePasseOubliePageQuery : IPageQuery<MotDePasseOubliePage>
    {

    }

    public class MotDePasseOubliePage : PageBase<MotDePasseOubliePageQuery>
    {
        public Form<MotDePasseOublieCommand, MotDePasseOublieConfirmationPageQuery> RecevoirEmail;
    }

    public class MotDePasseOubliePageModule : PageModule
    {
        public MotDePasseOubliePageModule()
        {
            HandlePage<MotDePasseOubliePageQuery, MotDePasseOubliePage>();
            ForMessage<MotDePasseOublieCommand>()
            .WithResponse<Empty>()
            .BuildModel<MotDePasseOublieConfirmationPageQuery>((c, r, m) => { });
        }
    }
}