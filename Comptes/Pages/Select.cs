using CocoriCore;

namespace Comptes
{
    public class Select<TQuery, TModel> where TQuery : IMessage
    {
        public bool IsSelect => true;
        public Select()
        {
            Source = new AsyncCall<TQuery, TModel[]>();
        }

        public AsyncCall<TQuery, TModel[]> Source;
        public TModel Selected;
    }

    public class ValueLabel<T>
    {
        public T Value;
        public string Label;
    }
}