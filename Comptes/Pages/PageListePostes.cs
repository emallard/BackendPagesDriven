using System;
using CocoriCore;

namespace Comptes
{
    public class PageListePostesQuery : IMessage<PageListePostes>
    {
    }

    public class PageListePostes
    {
        AsyncCall<IListQuery<PosteView>, PosteView[]> Postes;
    }

    public class PageListePostesModule
    {

    }
}