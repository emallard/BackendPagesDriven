using System;

namespace CocoriCore
{
    public struct ID<T>
    {
        public Guid Id;

        public ID(Guid id)
        {
            this.Id = id;
        }

        public ID(string input)
        {
            Id = Guid.Parse(input);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
        public override bool Equals(object obj)
        {
            return obj is ID<T> id &&
                   Id.Equals(id.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(ID<T> a, ID<T> b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(ID<T> a, ID<T> b)
        {
            return a.Id != b.Id;
        }
    }
}