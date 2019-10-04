namespace CocoriCore
{

    public interface IPageQuery : IMessage
    {

    }

    public interface IPageQuery<T> : IMessage<T>, IPageQuery
    {

    }
}