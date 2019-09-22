namespace CocoriCore
{
    public interface IView<T>
    {
        TypedId<T> Id { get; }
    }
}