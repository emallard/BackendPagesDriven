namespace CocoriCore
{
    public interface IView<T>
    {
        TId<T> Id { get; }
    }
}