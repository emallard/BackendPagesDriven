using CocoriCore.Page;

namespace MultiUser
{
    public class ScenarioInscriptionDeconnexion : IScenario<MultiUserAccueilPage>
    {
        public string Email;
        public string Password;
        public string Nom;
        public string Prenom;
        public BrowserFluent<MultiUserAccueilPage> Play(IBrowserFluent browserFluent)
        {
            if (Email == null)
                Email = "aa@aa.aa";
            if (Password == null)
                Password = "azerty";
            if (Nom == null)
                Nom = "DeNice";
            if (Prenom == null)
                Prenom = "Brice";

            return browserFluent.Display(new MultiUserAccueilPageQuery())
                .Follow(p => p.Menu.Inscription)
                .Fill(p => p.SInscrire.Command.Email, Email)
                .Fill(p => p.SInscrire.Command.Password, Password)
                .Fill(p => p.SInscrire.Command.PasswordConfirmation, Password)
                .Fill(p => p.SInscrire.Command.Nom, Nom)
                .Fill(p => p.SInscrire.Command.Prenom, Prenom)
                .Submit(p => p.SInscrire)
                .ThenFollow(r => r.UtilisateurPage)
                .Click(p => p.Menu.SeDeconnecter)
                .ThenFollow(r => r);
        }
    }
}