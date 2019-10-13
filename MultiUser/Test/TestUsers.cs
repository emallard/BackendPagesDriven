using System;
using System.Threading.Tasks;
using CocoriCore.Page;
using FluentAssertions;
using Xunit;

namespace MultiUser
{
    public class TestUser : TestBase
    {
        [Fact]
        public void InscriptionDeconnexionConnexion()
        {
            var user = CreateUser("user");

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

        /*
        [Fact]
        public void ImpossibleDeSeConnecterMauvaisMotDePasse()
        {
            var vendeur1 = CreateBrowser("vendeur1");

            var dashboard =
            vendeur1.Display(new Users_Inscription_Page_GET())
                .Submit(p => p.SInscrire,
                        m =>
                        {
                            m.Email = "aa@aa.aa";
                            m.Password = "azerty";
                            m.PasswordConfirmation = "azerty";
                            m.Nom = "DeNice";
                            m.Prenom = "Brice";
                        });

            var vendeur2 = CreateBrowser("vendeur2");
            var connexion = vendeur2.Follow(p => p.Connexion);

            Action a = () => connexion
                .Submit(p => p.SeConnecter,
                        m =>
                        {
                            m.Email = "aa@aa.aa";
                            m.Password = "mauvaiMotDePasse";
                        });

            a.Should().Throw<Exception>();

            Action b = () => connexion
                .Submit(p => p.SeConnecter,
                        m =>
                        {
                            m.Email = "bb@bb.bb";
                            m.Password = "azerty";
                        });

            b.Should().Throw<Exception>();
        }

        [Fact]
        public async Task MotDePasseOublie()
        {

            var user = CreateUser("vendeur");

            user.Display(new Accueil_Page_GET())
                .Play(new EnTantQueVendeur() { Email = "aa@aa.aa" })
                .Submit(p => p.MenuVendeur.SeDeconnecter, m => { })
                .ThenFollow(r => r.Accueil)
                .Follow(p => p.Connexion)
                .Follow(p => p.MotDePasseOublie)
                .Submit(p => p.RecevoirEmail,
                        m => m.Email = "aa@aa.aa")
                .ThenFollow(r => r);


            var emailMessage = await user.ReadEmail<EmailMotDePasseOublie>("aa@aa.aa");
            var saisieNouveauMotDePasse = emailMessage.Follow(body => body.Lien);
            saisieNouveauMotDePasse.Page.TokenDejaUtilise.Should().Be(false);
            saisieNouveauMotDePasse.Page.TokenExpire.Should().Be(false);

            var dashboard = saisieNouveauMotDePasse
                .Submit(p => p.ChangerMotDePasse,
                        m =>
                        {
                            m.Token = emailMessage.MailMessage.Body.Lien.Token;
                            m.MotDePasse = "nouveauPassw0rd";
                            m.Confirmation = "nouveauPassw0rd";
                        })
                .ThenFollow(r => r)
                .Submit(p => p.SeConnecter,
                        m =>
                        {
                            m.Email = "aa@aa.aa";
                            m.Password = "nouveauPassw0rd";
                        })
                .ThenFollow(r => r.PageDashboard);

            dashboard.Page.Should().NotBeNull();


            user.Comment("Reouverture de l'Email");
            saisieNouveauMotDePasse = emailMessage.Follow(body => body.Lien);
            saisieNouveauMotDePasse.Page.TokenDejaUtilise.Should().Be(true);
            saisieNouveauMotDePasse.Page.TokenExpire.Should().Be(false);
        }

        [Fact]
        public async Task MotDePasseOublieTokenExpire()
        {

            var user = CreateUser("vendeur");

            var confirmation =
            user.Display(new Accueil_Page_GET())
                .Play(new EnTantQueVendeur() { Email = "aa@aa.aa" })
                .Submit(p => p.MenuVendeur.SeDeconnecter,
                        m => { })
                .ThenFollow(r => r.Accueil)
                .Follow(p => p.Connexion)
                .Follow(p => p.MotDePasseOublie)
                .Submit(p => p.RecevoirEmail,
                        m => m.Email = "aa@aa.aa")
                .ThenFollow(r => r);

            user.Wait(new TimeSpan(2, 0, 0));

            var saisieNouveauMotDePasse =
            (await user.ReadEmail<EmailMotDePasseOublie>("aa@aa.aa"))
                       .Follow(body => body.Lien)
                       .Page;


            saisieNouveauMotDePasse.TokenDejaUtilise.Should().Be(false);
            saisieNouveauMotDePasse.TokenExpire.Should().Be(true);
        }
        */
    }
}