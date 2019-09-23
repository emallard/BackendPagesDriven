namespace CocoriCore
{

    public interface IPageQuery
    {

    }

    public interface IPageQuery<T> : IMessage<T>, IPageQuery
    {

    }
}