using System;

namespace CocoriCore
{
    public struct TId<T>
    {
        public Guid Id;

        public override bool Equals(object obj)
        {
            return obj is TId<T> id &&
                   Id.Equals(id.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(TId<T> a, TId<T> b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(TId<T> a, TId<T> b)
        {
            return a.Id != b.Id;
        }
    }
}