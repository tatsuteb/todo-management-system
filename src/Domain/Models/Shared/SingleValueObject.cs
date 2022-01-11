namespace Domain.Models.Shared
{
    public abstract class SingleValueObject<T> : IEquatable<SingleValueObject<T>>
    {
        public T Value { get; }

        protected SingleValueObject(T value)
        {
            Value = value;
        }

        public bool Equals(SingleValueObject<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SingleValueObject<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value ?? throw new DomainException("値を設定してください。"));
        }

        public static bool operator ==(SingleValueObject<T>? left, SingleValueObject<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SingleValueObject<T>? left, SingleValueObject<T>? right)
        {
            return !Equals(left, right);
        }
    }
}
