using System;
using System.Collections.Generic;
using System.Linq;
using CocoriCore;
using CocoriCore.Linq.Async;
using CocoriCore.Page;
using FluentAssertions;
using Xunit;

namespace Comptes
{
    public class ScenarioCreerPoste : IScenario<AccueilPage, PosteListPage>
    {

        public string[] NomPostes;
        public int NombreDePostes = 0;

        /*
        private readonly IRepository repository;
        public ScenarioCreerPoste(IRepository repository)
        {
            this.repository = repository;
        }*/

        public BrowserFluent<PosteListPage> Play(BrowserFluent<AccueilPage> browserFluent)
        {
            /*
            if (NomPostes == null)
            {
                var postesExistant = await (repository.Query<Poste>().ToArrayAsync()).Select(x => x.Nom).ToArray();
                for (var i = 0; i < NombreDePostes; ++i)
                {

                }
            }*/

            var listePostes = browserFluent
                .Follow(p => p.ListePostes);
            foreach (var n in this.NomPostes)
            {
                listePostes = Play(listePostes, n);
            }
            return listePostes;
        }

        public BrowserFluent<PosteListPage> Play(BrowserFluent<PosteListPage> browserFluent, string nom)
        {
            return browserFluent
                .Follow(p => p.NouveauPoste)
                .Fill(p => p.Creer.Command.Nom, nom)
                .Submit(p => p.Creer)
                .ThenFollow(r => r);
        }
    }
}