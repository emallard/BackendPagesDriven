using System;
using CocoriCore;
using FluentAssertions;
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
                .Follow(p => p.NouveauPoste)
                .Assert(p => p.Modele.Result.Nom.Should().Be("Voiture"))

                .Fill(p => p.Modele.Result.Nom, "Alimentation")
                .Submit(p => p.Creer)
                .ThenFollow(r => r)
                .Assert(p => p.Postes.Result,
                    x => x.Length.Should().Be(1),
                    x => x[0].Poste.Nom.Should().Be("Alimentation")
                );
        }
    }
}