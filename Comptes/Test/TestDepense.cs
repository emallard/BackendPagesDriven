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
                .Assert(p => p.Depense.Result,
                        depense => depense.Date.Should().Be(Get<IClock>().Today))

                .Fill(p => p.PosteSelect.Selected, p => p.PosteSelect.Source.Result.First(x => x.Label == "Voiture"))
                .Fill(p => p.Creer.Command.Description, "Plein d'essence")
                .Fill(p => p.Creer.Command.Montant, 30)
                .Fill(p => p.Creer.Command.Date, new DateTime(2000, 1, 1))
                .Submit(p => p.Creer)
                .ThenFollow(r => r)

                .Assert(p => p.Depenses.Result.Should().ContainSingle(
                    x => x.NomPoste == "Voiture"
                      && x.Description == "Plein d'essence"
                      && x.Date == new DateTime(2000, 1, 1)
                      && x.Montant == 30
                      ));
        }

        [Fact]
        public void ModifierDepense()
        {
            var user = CreateUser("moi");

            user
                .Play(Get<ScenarioCreerDepense>())

                .Display(new AccueilPageQuery())
                .Follow(p => p.ListeDepenses)
                .Follow(p => p.Depenses.Result[0].Modifier)
                .Fill(p => p.PosteSelect.Selected, p => p.PosteSelect.Source.Result.First(x => x.Label == "Alimentation"))
                .Fill(p => p.Enregistrer.Command.Description, "Updated description")
                .Submit(p => p.Enregistrer)
                .ThenFollow(r => r);
        }
    }
}
