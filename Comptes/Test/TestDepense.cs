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
                .Play(Get<ScenarioCreerPoste>(s => s.NomPostes = new string[] { "Voiture", "Alimentation" }))

                .Display(new AccueilPageQuery())
                .Follow(p => p.ListeDepenses)
                .Follow(p => p.NouvelleDepense)

                .Assert(p => p.PosteSelect.Source.Result,
                    source => source.Should().Contain(x => x.Label == "Voiture"),
                    source => source.Should().Contain(x => x.Label == "Alimentation"))

                .Fill(p => p.PosteSelect.Selected, p => p.PosteSelect.Source.Result.First(x => x.Label == "Voiture"))
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