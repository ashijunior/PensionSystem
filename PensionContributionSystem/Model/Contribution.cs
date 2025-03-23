namespace PensionContributionSystem.Model
{
    public class Contribution
    {
        public int ContributionID { get; set; }
        public int MemberID { get; set; }
        public ContributionType ContributionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ContributionDate { get; set; }
        public bool IsValidated { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
