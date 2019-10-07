namespace Comptes
{
    public class MenuComponent
    {
        public bool IsMenu => true;
        public AccueilPageQuery Accueil = new AccueilPageQuery();
        public PosteListPageQuery ListePostes = new PosteListPageQuery();
        public DepenseListPageQuery ListeDepenses = new DepenseListPageQuery();
    }
}