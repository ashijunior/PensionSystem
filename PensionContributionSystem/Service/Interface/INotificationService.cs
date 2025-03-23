namespace PensionContributionSystem.Service.Interface
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int memberId, string message);
    }
}
