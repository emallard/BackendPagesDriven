namespace CocoriCore
{
    public interface IMyEntity<T> : IEntity
    {
        TypedId<T> TId { get; }
    }
}