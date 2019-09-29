namespace CocoriCore
{
    public interface IView<T>
    {
        ID<T> Id { get; }
    }
}