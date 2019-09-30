using CocoriCore;

namespace Comptes
{
    public class Select<TQuery, TModel> where TQuery : IMessage
    {
        public Select()
        {
            Source = new AsyncCall<TQuery, TModel[]>();
        }

        public AsyncCall<TQuery, TModel[]> Source;
        public TModel Selected;
    }
}