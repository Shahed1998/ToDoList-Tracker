using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models.Business_Entities;
using Web.Models.General_Entities;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class TrackingController : BaseController
    {
        private readonly ITrackerService _trackerService;
        private readonly SignInManager<User> _signInManager;

        public TrackingController(ITrackerService trackerService, SignInManager<User> signInManager)
        {
            _trackerService = trackerService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(bool? IsSaved, int pageNumber = 1, int pageSize = 5,
            int count = 0, bool prevPage = false, string message = "")
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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }

            var trackingList = await _trackerService.GetAllTrackers(pageNumber, pageSize, userId);

            CompositeTrackerViewModel model = new CompositeTrackerViewModel();

            model.Pager = trackingList.Item1;

            var end = model.Pager.Count();

            ViewBag.IsSaved = IsSaved;
            ViewBag.Achievements = trackingList.Item2;
            ViewBag.Count = count;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Message = message;

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new CreateTrackerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrackerViewModel createModel)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { IsSaved = false, message = "Failed to create" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }

            var model = new TrackerViewModel();

            model.Completed = createModel.Completed;
            model.Planned = createModel.Planned;

            model.UserId = userId;

            bool ts = await _trackerService.AddTracker(model);

            return RedirectToAction("Index", new { IsSaved = ts });
        }

        [Route("Tracking/Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            bool isSaved = await _trackerService.Delete(Id);

            return RedirectToAction("Index", new { IsSaved = isSaved });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var model = await _trackerService.GetTrackerById(Id);
            return PartialView("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TrackerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { IsSaved = false, message="Failed to edit" });
            }

            var isUpdated = await _trackerService.Update(model);

            if (!isUpdated)
            {
                ModelState.AddModelError("isSaved", "Unable to update");
                return PartialView("Edit", model);
            }

            return RedirectToAction("Index", new { IsSaved = true });
        }

        public async Task<IActionResult> DeleteAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }

            var isSuccessfullyDeleted = await _trackerService.DeleteAll(userId);

            return RedirectToAction("Index", new { IsSaved = isSuccessfullyDeleted });
        }


        public IActionResult LoadPartialView(int viewNumber)
        {
            if (viewNumber == 2) return PartialView("_Report", new TrackerViewModel());
            return PartialView("_Create", new TrackerViewModel());
        }
    }
}
