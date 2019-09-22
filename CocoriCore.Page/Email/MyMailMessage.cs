namespace CocoriCore.Page
{
    public interface IMyMailMessage
    {
        string From { get; }
        string To { get; }
        string Subject { get; }
        object Body { get; }
    }

    public class MyMailMessage<T> : IMyMailMessage
    {
        public MyMailMessage()
        {

        }

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }

        public T Body { get; set; }

        object IMyMailMessage.Body => this.Body;
    }
}