using Microsoft.AspNetCore.Mvc;
using Web.Models.Business_Entities;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class TrackingController : Controller
    {
        private readonly ITrackerService _trackerService;

        public TrackingController(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public async Task<IActionResult> Index(bool? IsSaved, int pageNumber=1, int pageSize=5, int count = 0, bool prevPage = false) 
        {

            if (prevPage)
            {
                count = count - pageSize - 1;
            }

            if (pageNumber <= 1) 
            {
                pageNumber = 1;
                count = 0;
            }

           
            var trackingList = await _trackerService.GetAllTrackers(pageNumber, pageSize);

            CompositeTrackerViewModel model = new CompositeTrackerViewModel();

            model.Pager = trackingList.Item1;

            var end = model.Pager.Count();

            ViewBag.IsSaved = IsSaved;
            ViewBag.Achievements = trackingList.Item2;
            ViewBag.Count = count;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(TrackerViewModel model)
        {
            bool ts = await _trackerService.AddTracker(model);

            return RedirectToAction("Index", new { IsSaved = ts });
        }

        [Route("Tracking/Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            bool isSaved = await _trackerService.Delete(Id);

            return RedirectToAction("Index", new { IsSaved = isSaved });
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> GetEdit(int Id)
        {
            var model = await _trackerService.GetTrackerById(Id);
            return PartialView("Edit", model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> PostEdit(TrackerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Edit", model);
            }

            var isUpdated = await _trackerService.Update(model);

            if(!isUpdated) 
            {
                ModelState.AddModelError("isSaved", "Unable to update");
                return PartialView("Edit", model);
            }

            return RedirectToAction("Index", new { IsSaved = true });
        }

        public async Task<IActionResult> DeleteAll()
        {
            var isSuccessfullyDeleted = await _trackerService.DeleteAll();

            return RedirectToAction("Index", new { IsSaved = isSuccessfullyDeleted });
        }


        public IActionResult LoadPartialView(int viewNumber)
        {
            if (viewNumber == 2) return PartialView("_Report", new TrackerViewModel());
            return PartialView("_Create", new TrackerViewModel());
        }
    }
}
