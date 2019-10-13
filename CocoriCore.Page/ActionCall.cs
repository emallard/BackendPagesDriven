namespace CocoriCore
{
    public class ActionCall<TQuery, TModel> : AsyncCall<TQuery, TModel> where TQuery : IMessage
    {
        public ActionCall()
        {
            CallType = AsyncCallType.Action;
        }
    }
}