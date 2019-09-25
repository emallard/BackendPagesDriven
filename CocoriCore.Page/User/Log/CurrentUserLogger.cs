namespace CocoriCore.Page
{
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
            log.UserName = this.Id;
            this.logger.Log(log);
        }

        public void SetUserId(string id)
        {
            this.Id = id;
        }
    }
}
