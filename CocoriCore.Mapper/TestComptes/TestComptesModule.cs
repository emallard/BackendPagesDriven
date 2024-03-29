﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore.Mapper.Comptes
{

    public class TestComptesModule : INewMapperModule
    {
        public void Load(INewMapper mapper)
        {
            mapper.AddCreate<PosteCreate, Poste>((x, y) =>
            {
                y.Nom = x.Nom;
            });

            mapper.AddUpdate<PosteUpdate, Poste>(x => x.Id, (x, y) =>
            {
                y.Nom = x.Nom;
            });

            mapper.AddView<Poste, PosteView>((x, y) =>
            {
                y.Id = x.Id;
                y.Nom = x.Nom;
            });

            mapper.AddCreate<DepenseCreate, Depense>((x, y) =>
            {
                y.Description = x.Description;
                y.Montant = x.Montant;
                y.IdPoste = x.IdPoste;
            });

            /*
            mapper.AddFrom<DepenseView, Depense, Poste>(
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