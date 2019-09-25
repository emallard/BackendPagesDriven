namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IMessage<HomePage>
    {

    }

    public class HomePage
    {
        public AsyncCall<SvgResponse> PageGraph;
        public AsyncCall<SvgResponse> EntityGraph;
        /*
        public AsyncCall<TestListReponseItem[]> Tests;
        public AsyncCall<EntityTypeListReponseItem[]> Entities;
        public AsyncCall<PageTypeListReponseItem[]> Pages;
        */
        public Form<RunTestCommand, HomePageQuery> RunTests;
    }
}