using Web.Models.Business_Entities;
using Web.Models.General_Entities;

namespace Web.Repositories.Interfaces
{
    public interface ITrackerRepository
    {
        Task<bool> Add(Tracker model);
        Task<(Pager<Tracker>, decimal?)> GetAll(int pageNumber = 1, int pageSize = 10);
        Task<Tracker?> GetById(int id);
        Task Delete(int Id);
        Task Edit(Tracker model);
    }
}
