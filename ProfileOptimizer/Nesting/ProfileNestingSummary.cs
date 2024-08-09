namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingSummary
    {
        public double AverageUtilization { get; set; }

        public int MaterialPiece { get; set; }

        public int PartPiece { get; set; }

        public List<ProfileMaterial> Materials { get; set; } = [];

        public List<ProfileNestingPlan> Plans { get; set; } = [];

        public double TotalLength { get; set; }

        public double TotalRemainLength { get; set; }
    }
}