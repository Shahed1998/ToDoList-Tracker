using Microsoft.AspNetCore.Mvc;
using web.Data;
using Web.Models.General_Entities;

namespace Web.ViewComponents.Menu
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;
        public MenuViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IViewComponentResult Invoke()
        {
            var result = _dataContext.MenuConfigs.Where(x => x.IsActive).ToList();
            return View(result);
        }
    }
}
