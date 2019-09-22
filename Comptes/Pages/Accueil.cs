using System;
using CocoriCore;

namespace Comptes
{
    public class PageAccueilQuery : IMessage<PageAccueil>
    {
    }

    public class PageAccueil
    {
        PageListePostesQuery ListePostes;
    }

    public class PageAccueilModule
    {

    }
}