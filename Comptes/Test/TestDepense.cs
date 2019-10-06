using System;
using System.Linq;
using CocoriCore;
using FluentAssertions;
using Xunit;

namespace Comptes
{
    public class TestDepense : TestBase
    {
        [Fact]
        public void CreerDepense()
        {
            var user = CreateUser("moi");

            user
                .Display(new AccueilPageQuery())
                .Follow(p => p.ListePostes)
                .Follow(p => p.NouveauPoste)
                .Fill(p => p.Creer.Command.Nom, "Voiture")
                .Submit(p => p.Creer)
                .ThenFollow(r => r)

                .Display(new AccueilPageQuery())
                .Follow(p => p.ListeDepenses)
                .Follow(p => p.NouvelleDepense)

                .Assert(p => p.PosteSelect.Source.Result.Should().ContainSingle(x => x.Label == "Voiture"))

                .Fill(p => p.PosteSelect.Selected, p => p.PosteSelect.Source.Result.First())
                .Fill(p => p.Creer.Command.Description, "Plein d'essence")
                .Fill(p => p.Creer.Command.Montant, 30)
                .Submit(p => p.Creer)
                /*
                .ThenFollow(r => r)
                /*
                .Assert(p => p.Depenses.Result.Should().ContainSingle(
                            x => x.Depense.NomPoste == ""
                              && x.Depense.Description == "Plein d'essence"
                              && x.Depense.Montant == 30))
                .Follow(p => p.Depenses.Result[0].Lien)
                .Assert(p => p.Depense.Result,
                            x => x.NomPoste.Should().Be("Voiture"),
                            x => x.Description.Should().Be("Plein d'essence"),
                            x => x.Montant.Should().Be(30))
                */
                ;


        }
    }
}