namespace CocoriCore
{
    public class ServerCall<TQuery, TModel> : AsyncCall<TQuery, TModel> where TQuery : IMessage
    {
        public ServerCall()
        {
            CallType = AsyncCallType.Server;
        }
    }
}