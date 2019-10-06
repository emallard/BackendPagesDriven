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
                .Submit(p => p.Creer);


            // TODO : quand il y a des valeurs par defaut. par exemple date.today, 
            //        faire un test où on remplit, un test où on remplit pas

        }
    }
}
