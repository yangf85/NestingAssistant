namespace ProfileOptimizer.Nesting
{
    public class UsageProfilePart : IComparable<UsageProfilePart>, IEquatable<UsageProfilePart>
    {
        public int Id { get; set; }

        public double Length { get; set; }

        public int Index { get; set; }

        public string Type { get; set; }

        public string Label { get; set; }

        public bool IsUsed { get; set; }

        public int CompareTo(UsageProfilePart? other)
        {
            if (other is null)
            {
                return 1; // 当前实例大于null
            }

            // 先按Type比较
            int typeComparison = string.Compare(Type, other.Type, StringComparison.Ordinal);
            if (typeComparison != 0)
            {
                return typeComparison;
            }

            // 然后按Label比较
            int labelComparison = string.Compare(Label, other.Label, StringComparison.Ordinal);
            if (labelComparison != 0)
            {
                return labelComparison;
            }

            // 最后按Length比较
            return Length.CompareTo(other.Length);
        }

        public bool Equals(UsageProfilePart? other)
        {
            if (other is null)
            {
                return false;
            }

            return string.Equals(Type, other.Type, StringComparison.Ordinal) &&
                   string.Equals(Label, other.Label, StringComparison.Ordinal) &&
                   Length == other.Length;
        }

        public override bool Equals(object obj)
        {
            if (obj is UsageProfilePart other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + (Type?.GetHashCode(StringComparison.Ordinal) ?? 0);
            hash = hash * 31 + (Label?.GetHashCode(StringComparison.Ordinal) ?? 0);
            hash = hash * 31 + Length.GetHashCode();
            return hash;
        }
    }
}