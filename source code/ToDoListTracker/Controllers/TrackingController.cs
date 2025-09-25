using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using ToDoListTracker.Models.Business_Entities;
using Web.Models.Business_Entities;
using Web.Models.General_Entities;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class TrackingController : BaseController
    {
        private readonly ITrackerService _trackerService;
        private readonly SignInManager<User> _signInManager;

        public TrackingController(ITrackerService trackerService, SignInManager<User> signInManager)
        {
            _trackerService = trackerService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(IndexVM vm)
        {

            if (TempData["IndexParams"] is string json)
            {
                vm = JsonSerializer.Deserialize<IndexVM>(json) ?? new IndexVM();
            }
            else if(vm is null)
            {
                vm = new IndexVM();
            }

            if (vm.prevPage)
            {
                vm.count = vm.count - vm.pageSize - 1;
            }

            if (vm.pageNumber <= 1)
            {
                vm.pageNumber = 1;
                vm.count = 0;
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }

            var trackingList = await _trackerService.GetAllTrackers(vm.pageNumber, vm.pageSize, userId);

            CompositeTrackerViewModel model = new CompositeTrackerViewModel();

            model.Pager = trackingList.Item1;
            model.IndexVM = vm;
            model.Achievements = trackingList.Item2 ?? decimal.Zero;
            model.Title = "All Trackers";

            var end = model.Pager.Count();
            ViewBag.Message =  vm.message;

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
                TempData["IndexParams"] = JsonSerializer.Serialize(new IndexVM() { IsSaved = false, Notify = true, message= "Failed to create" });

                return RedirectToAction("Index");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }

            var model = new TrackerViewModel();

            model.Completed = createModel.IsFlagged ? 0 : createModel.Completed;
            model.Planned = createModel.Planned;
            model.IsFlagged = createModel.IsFlagged;

            model.UserId = userId;

            bool ts = await _trackerService.AddTracker(model);

            TempData["IndexParams"] = JsonSerializer.Serialize(new IndexVM() { IsSaved = ts, Notify = true, message = "Success" });

            return RedirectToAction("Index");
        }

        [Route("Tracking/Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            bool isSaved = await _trackerService.Delete(Id);
            var message = isSaved == true ? "Success" : "Failed";

            TempData["IndexParams"] = JsonSerializer.Serialize(new IndexVM() { IsSaved = isSaved, Notify = true, message = message });

            return RedirectToAction("Index");
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
                TempData["IndexParams"] = JsonSerializer.Serialize(new IndexVM() { IsSaved = false, Notify = true, message = "Failed to edit" });
                return RedirectToAction("Index");
            }

            if (model.IsFlagged == true) model.Completed = 0;

            var isUpdated = await _trackerService.Update(model);

            if (!isUpdated)
            {
                ModelState.AddModelError("isSaved", "Unable to update");
                return PartialView("Edit", model);
            }

            TempData["IndexParams"] = JsonSerializer.Serialize(new IndexVM() { IsSaved = true, Notify = true, message = "Success" });

            return RedirectToAction("Index");
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
            var message = isSuccessfullyDeleted ? "Success" : "Failed";

            TempData["IndexParams"] = JsonSerializer.Serialize(new IndexVM() { IsSaved = isSuccessfullyDeleted, Notify = true, message= message });

            return RedirectToAction("Index");
        }


        public IActionResult LoadPartialView(int viewNumber)
        {
            if (viewNumber == 2) return PartialView("_Report", new TrackerViewModel());
            return PartialView("_Create", new TrackerViewModel());
        }
    }
}
