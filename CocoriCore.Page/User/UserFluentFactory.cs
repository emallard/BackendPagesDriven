namespace CocoriCore.Page
{
    public class UserFluentFactory
    {
        public UserFluentFactory(UserFluent userFluent)
        {
            UserFluent = userFluent;
        }

        public UserFluent UserFluent { get; }
    }
}
