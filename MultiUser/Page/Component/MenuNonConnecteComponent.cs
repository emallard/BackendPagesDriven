namespace MultiUser
{
    public class MenuNonConnecteComponent
    {
        public bool IsMenu => true;
        public MultiUserAccueilPageQuery Accueil = new MultiUserAccueilPageQuery();
        public ConnexionPageQuery Connexion = new ConnexionPageQuery();
        public InscriptionPageQuery Inscription = new InscriptionPageQuery();
    }
}