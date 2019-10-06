using System;
using System.Linq;
using CocoriCore;
using FluentAssertions;
using Xunit;

namespace Comptes
{
    public class TestPoste : TestBase
    {
        [Fact]
        public void CreerPoste()
        {
            var user = CreateUser("moi");

            user
                .Display(new AccueilPageQuery())
                .Follow(p => p.ListePostes)
                .Follow(p => p.NouveauPoste)
                .Assert(p => p.Poste.Result.Nom.Should().Be("Voiture"))

                .Fill(p => p.Creer.Command.Nom, "Alimentation")
                .Submit(p => p.Creer)
                .ThenFollow(r => r)
                .Assert(p => p.Postes.Result,
                        x => x.Length.Should().Be(1),
                        x => x[0].Poste.Nom.Should().Be("Alimentation"));
        }

        [Fact]
        public void ModifierPoste()
        {
            var user = CreateUser("moi");

            user
                .Display(new AccueilPageQuery())
                .Play(Get<ScenarioCreerPoste>(x => x.NomPostes = new string[] { "Alimentation", "Vacances" }))

                .Follow(p => p.Postes.Result[0].Lien)
                .Follow(p => p.Modifier)
                .Assert(p => p.Poste.Result.Nom.Should().Be("Alimentation"))
                .Fill(p => p.Enregistrer.Command.Nom, "Habits")
                .Submit(p => p.Enregistrer)
                .ThenFollow(r => r)
                .Assert(p => p.Postes.Result,
                        x => x.Length.Should().Be(2),
                        x => x[0].Poste.Nom.Should().Be("Habits"),
                        x => x[1].Poste.Nom.Should().Be("Vacances"));
        }

    }
}