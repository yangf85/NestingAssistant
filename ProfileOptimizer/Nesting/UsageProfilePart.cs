namespace ProfileOptimizer.Nesting
{
    public class UsageProfilePart : IComparable<UsageProfilePart>
    {
        public int Id { get; set; }

        public double Length { get; set; }

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
    }
}