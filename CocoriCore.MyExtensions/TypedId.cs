using System;

namespace CocoriCore
{
    public struct TypedId<T>
    {
        public Guid Id;

        public override bool Equals(object obj)
        {
            return obj is TypedId<T> id &&
                   Id.Equals(id.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(TypedId<T> a, TypedId<T> b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(TypedId<T> a, TypedId<T> b)
        {
            return a.Id != b.Id;
        }
    }
}