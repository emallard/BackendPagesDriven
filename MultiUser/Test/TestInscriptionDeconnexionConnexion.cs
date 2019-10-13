using System;
using System.Threading.Tasks;
using CocoriCore.Page;
using FluentAssertions;
using Xunit;

namespace MultiUser
{
    public class TestInscriptionDeconnexionConnexion : TestBase
    {
        [Fact]
        public void InscriptionDeconnexionConnexion()
        {
            var user = CreateUser();

            var dashboard =
            user.Display(new MultiUserAccueilPageQuery())
                .Follow(p => p.Menu.Inscription)
                .Fill(p => p.SInscrire.Command.Email, "aa@aa.aa")
                .Fill(p => p.SInscrire.Command.Password, "azerty")
                .Fill(p => p.SInscrire.Command.PasswordConfirmation, "azerty")
                .Fill(p => p.SInscrire.Command.Nom, "DeNice")
                .Fill(p => p.SInscrire.Command.Prenom, "Brice")
                .Submit(p => p.SInscrire)
                .ThenFollow(r => r.UtilisateurPage)
                .Assert(p => p.Profile.Result, x =>
                {
                    x.Nom.Should().Be("DeNice");
                    x.Prenom.Should().Be("Brice");
                })


                .Click(p => p.Menu.SeDeconnecter)
                .ThenFollow(r => r)
                .Follow(p => p.Menu.Connexion)
                .Fill(p => p.SeConnecter.Command.Email, "aa@aa.aa")
                .Fill(p => p.SeConnecter.Command.Password, "azerty")
                .Submit(p => p.SeConnecter)
                .ThenFollow(r => r.UtilisateurPage)

                .Assert(p => p.Profile.Result, x =>
                 {
                     x.Nom.Should().Be("DeNice");
                     x.Prenom.Should().Be("Brice");
                 });
        }

        public void DeconnexionAccesAuxUrlPriveesImpossible()
        {
            var user = CreateUser();

            var dashboard =
            user.Display(new MultiUserAccueilPageQuery())
                .Follow(p => p.Menu.Inscription)
                .Fill(p => p.SInscrire.Command.Email, "aa@aa.aa")
                .Fill(p => p.SInscrire.Command.Password, "azerty")
                .Fill(p => p.SInscrire.Command.PasswordConfirmation, "azerty")
                .Fill(p => p.SInscrire.Command.Nom, "DeNice")
                .Fill(p => p.SInscrire.Command.Prenom, "Brice")
                .Submit(p => p.SInscrire)
                .ThenFollow(r => r.UtilisateurPage)
                .Assert(p => p.Profile.Result, x =>
                {
                    x.Nom.Should().Be("DeNice");
                    x.Prenom.Should().Be("Brice");
                })
                .Click(p => p.Menu.SeDeconnecter);

            Action a = () => user.Display(new UtilisateurPageQuery());
            a.Should().Throw<Exception>();
        }


        [Fact]
        public void ValidationPageConnexion()
        {
            var user1 = CreateUser("user1");

            user1.Display(new MultiUserAccueilPageQuery())
                .Follow(p => p.Menu.Inscription)
                .Fill(p => p.SInscrire.Command.Email, "user1@aa.aa")
                .Fill(p => p.SInscrire.Command.Password, "bonMotDePasse")
                .Fill(p => p.SInscrire.Command.PasswordConfirmation, "bonMotDePasse")
                .Fill(p => p.SInscrire.Command.Nom, "DeNice")
                .Fill(p => p.SInscrire.Command.Prenom, "Brice")
                .Submit(p => p.SInscrire)
                .ThenFollow(r => r.UtilisateurPage)
                .Click(p => p.Menu.SeDeconnecter)
                .ThenFollow(r => r)

                .Follow(p => p.Menu.Connexion)
                .Fill(p => p.SeConnecter.Command.Email, "user1@aa.aa")
                .Fill(p => p.SeConnecter.Command.Password, "mauvaisMotDePasse")
                .SubmitShouldFail(p => p.SeConnecter)

                .Fill(p => p.SeConnecter.Command.Email, "user1@bb.bb")
                .Fill(p => p.SeConnecter.Command.Password, "bonMotDePasse")
                .SubmitShouldFail(p => p.SeConnecter);
        }
    }
}