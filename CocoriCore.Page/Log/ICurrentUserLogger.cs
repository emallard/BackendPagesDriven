namespace CocoriCore.Page
{
    public interface ICurrentUserLogger
    {
        void Log(UserLog log);
        void SetUserId(string id);
    }
}
