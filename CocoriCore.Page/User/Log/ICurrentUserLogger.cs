namespace CocoriCore.Page
{
    public interface ICurrentUserLogger
    {
        void Log(UserLog log);
        void SetUserId(string id);
    }

    public class CurrentUserLogger : ICurrentUserLogger
    {
        private readonly IUserLogger logger;
        private string Id;

        public CurrentUserLogger(IUserLogger logger)
        {
            this.logger = logger;
        }

        public void Log(UserLog log)
        {
            log.Id = this.Id;
            this.logger.Log(log);
        }

        public void SetUserId(string id)
        {
            this.Id = id;
        }
    }
}
