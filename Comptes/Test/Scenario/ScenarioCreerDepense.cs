using System;
using System.Linq;
using CocoriCore.Page;

namespace Comptes
{
    public class ScenarioCreerDepense : IScenario<AccueilPage>
    {
        private readonly ScenarioCreerPoste scenarioCreerPoste;

        public ScenarioCreerDepense(
            ScenarioCreerPoste scenarioCreerPoste
        )
        {
            this.scenarioCreerPoste = scenarioCreerPoste;
        }

        public BrowserFluent<AccueilPage> Play(IBrowserFluent browserFluent)
        {
            scenarioCreerPoste.NomPostes = new string[] { "Voiture", "Alimentation" };

            return browserFluent
                .Display(new AccueilPageQuery())
                .Play(scenarioCreerPoste)

                .Display(new AccueilPageQuery())
                .Follow(p => p.ListeDepenses)
                .Follow(p => p.NouvelleDepense)

                .Fill(p => p.PosteSelect.Selected, p => p.PosteSelect.Source.Result.First(x => x.Label == "Voiture"))
                .Fill(p => p.Creer.Command.Description, "Plein d'essence")
                .Fill(p => p.Creer.Command.Montant, 30)
                .Fill(p => p.Creer.Command.Date, new DateTime(2000, 1, 1))
                .Submit(p => p.Creer)
                .ThenFollow(r => r)
                .Display(new AccueilPageQuery());
        }
    }
}