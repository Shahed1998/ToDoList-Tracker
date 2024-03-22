using Web.Models.General_Entities;

namespace Web.Repositories.Interfaces
{
    public interface ITrackerRepository
    {
        Task<bool> Add(Tracker model);
        Task<IEnumerable<Tracker>> GetAll();
        Task Delete(int Id);
    }
}
