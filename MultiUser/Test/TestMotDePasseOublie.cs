using System;
using System.Threading.Tasks;
using CocoriCore.Page;
using FluentAssertions;
using Xunit;

namespace MultiUser
{
    public class TestMotDePasseOublie : TestBase
    {


        [Fact]
        public async Task MotDePasseOublie()
        {

            var user = CreateUser("vendeur");

            user.Play(Get<ScenarioInscriptionDeconnexion>(x => x.Email = "aa@aa.aa"))
                .Follow(p => p.Menu.Connexion)
                .Follow(p => p.MotDePasseOublie)
                .Fill(p => p.RecevoirEmail.Command.Email, "aa@aa.aa")
                .Submit(p => p.RecevoirEmail)
                .ThenFollow(r => r);


            var emailMessage = await user.ReadEmail<EmailMotDePasseOublie>("aa@aa.aa");
            emailMessage
                .Follow(body => body.Lien)
                .Assert(p =>
                {
                    p.Token.Result.Utilisé.Should().Be(false);
                    p.Token.Result.Expiré.Should().Be(false);
                })
                .Fill(p => p.ChangerMotDePasse.Command.MotDePasse, "nouveauPassw0rd")
                .Fill(p => p.ChangerMotDePasse.Command.Confirmation, "nouveauPassw0rd")
                .Submit(p => p.ChangerMotDePasse)
                .ThenFollow(r => r)

                .Fill(p => p.SeConnecter.Command.Email, "aa@aa.aa")
                .Fill(p => p.SeConnecter.Command.Password, "nouveauPassw0rd")

                .Submit(p => p.SeConnecter)
                .ThenFollow(r => r.UtilisateurPage);


            user.Comment("Réutilisation de l'email de saisie de mot de passe");
            emailMessage
                .Follow(body => body.Lien)
                .Assert(p =>
                {
                    p.Token.Result.Utilisé.Should().Be(true);
                    p.Token.Result.Expiré.Should().Be(false);
                });
        }

        [Fact]
        public async Task MotDePasseOublieTokenExpire()
        {

            var user = CreateUser();

            user.Play(Get<ScenarioInscriptionDeconnexion>(x => x.Email = "aa@aa.aa"))
                .Follow(p => p.Menu.Connexion)
                .Follow(p => p.MotDePasseOublie)
                .Fill(p => p.RecevoirEmail.Command.Email, "aa@aa.aa")
                .Submit(p => p.RecevoirEmail)
                .ThenFollow(r => r);

            user.Wait(TimeSpan.FromHours(2));

            var saisieNouveauMotDePasse =
            (await user.ReadEmail<EmailMotDePasseOublie>("aa@aa.aa"))
                       .Follow(body => body.Lien)
                       .Assert(p =>
                {
                    p.Token.Result.Utilisé.Should().Be(false);
                    p.Token.Result.Expiré.Should().Be(true);
                });
        }
    }
}