namespace CocoriCore
{
    public class OnInitCall<TQuery, TModel> : AsyncCall<TQuery, TModel> where TQuery : IMessage
    {
        public OnInitCall()
        {
            CallType = AsyncCallType.OnInit;
        }
    }
}