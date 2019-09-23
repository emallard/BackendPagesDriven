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
            var pageNouveauPoste = user
                .Display(new PageAccueilQuery())
                .Follow(p => p.ListePostes)
                .Follow(p => p.NouveauPoste);

            var modele = pageNouveauPoste.Page.Modele.Result;
            modele.Nom.Should().Be("Voiture");

            var pageListePostes = pageNouveauPoste
                .Fill(p => p.Modele.Result.Nom, "Bijoux")
                .Submit(p => p.Creer)
                .ThenFollow(r => r);

            var postes = pageListePostes.Page.Postes.Result;
            postes.Length.Should().Be(1);
            postes[0].Poste.Nom.Should().Be("Bijoux");
        }
    }
}