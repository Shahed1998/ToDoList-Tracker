namespace Web.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ITrackerRepository TrackerRepository { get; }
        Task<bool> SaveAsync();
    }
}
