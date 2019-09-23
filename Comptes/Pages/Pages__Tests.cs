using System;
using CocoriCore;
using Xunit;

namespace Comptes
{
    public class Pages__Tests : TestBase
    {
        [Fact]
        public void Test1()
        {
            var user = CreateUser("moi");
            user
                .Display(new PageAccueilQuery())
                .Follow(p => p.ListePostes)
                .Follow(p => p.NouveauPoste);

        }
    }
}