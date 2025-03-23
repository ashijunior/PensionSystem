namespace PensionContributionSystem.Model
{
    public class Employer
    {
        public int EmployerID { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
