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

        public async Task<IActionResult> Index(bool IsSaved = false)

        {
            var trackingList = await _trackerService.GetAllTrackers();

            ViewBag.IsSaved = IsSaved;
            ViewBag.TrackingList = trackingList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(TrackerViewModel model)
        {

            bool ts = await _trackerService.AddTracker(model);
            var trackingList = await _trackerService.GetAllTrackers();

            ViewBag.TrackingList = trackingList;
            ViewBag.IsSaved = ts;

            if (ts)
            {
                ModelState.Clear();
                model = new TrackerViewModel();
            }

            return View(model);
        }

        [Route("Tracking/Edit/{Id}")]
        public IActionResult Edit(string Id)
        {
            throw new NotImplementedException();
        }

        [Route("Tracking/Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            bool isSaved = await _trackerService.Delete(Id);
            return RedirectToAction("Index", new { IsSaved = isSaved });
        }
    }
}
