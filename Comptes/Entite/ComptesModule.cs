using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CocoriCore;

namespace Comptes
{

    public class ComptesModule : INewMapperModule
    {
        public void Load(INewMapper mapper)
        {
            mapper.AddView<PosteCreateDefault>(x => x.Nom = "Voiture");

            mapper.AddView<Poste, PosteView>((x, y) =>
            {
                y.Id = x.Id;
                y.Nom = x.Nom;
            });

            /*
            mapper.WithRepository()
            {
                
            }
            */

            /*
            mapper.AddJoin<DepenseView, Depense, Poste>(
                x => x.Id,
                (x, y) =>
                {
                    x.Id = y.Id;
                    x.IdPoste = y.IdPoste;
                    x.Description = y.Description;
                    x.Montant = y.Montant;
                },
                x => x.IdPoste,
                (x, z) =>
                {
                    x.NomPoste = z.Nom;
                });
                */
        }
    }
}