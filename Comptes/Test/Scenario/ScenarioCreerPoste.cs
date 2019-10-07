using System.Collections.Generic;
using System.Linq;
using CocoriCore;
using CocoriCore.Linq.Async;
using CocoriCore.Page;
using FluentAssertions;
using Xunit;

namespace Comptes
{

    public class ScenarioCreerPoste : IScenario<AccueilPage>
    {

        public string[] NomPostes;
        public int NombreDePostes = 0;

        /*
        private readonly IRepository repository;
        public ScenarioCreerPoste(IRepository repository)
        {
            this.repository = repository;
        }*/

        public BrowserFluent<AccueilPage> Play(IBrowserFluent browserFluent)
        {
            /*
            if (NomPostes == null)
            {
                var postesExistant = await (repository.Query<Poste>().ToArrayAsync()).Select(x => x.Nom).ToArray();
                for (var i = 0; i < NombreDePostes; ++i)
                {

                }
            }*/
            BrowserFluent<AccueilPage> result = null;
            foreach (var n in this.NomPostes)
            {
                result = Play(browserFluent, n);
            }
            return result;
        }

        public BrowserFluent<AccueilPage> Play(IBrowserFluent browserFluent, string nom)
        {
            return browserFluent
                .Display(new AccueilPageQuery())
                .Follow(p => p.ListePostes)
                .Follow(p => p.NouveauPoste)
                .Fill(p => p.Creer.Command.Nom, nom)
                .Submit(p => p.Creer)
                .ThenFollow(r => r)
                .Display(new AccueilPageQuery());
        }
    }
}