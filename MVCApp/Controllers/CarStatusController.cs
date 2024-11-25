using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin", "User")]
    [Route("car-status")]
    [ApiController]
    public class CarStatusController : BaseController
    {
        private readonly ICarStatusService _carStatusService;

        public CarStatusController(ICarStatusService carStatusService)
        {
            _carStatusService = carStatusService;
        }

        [HttpGet("", Name = "cars-statuses")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters, string? carStatusFilter)
        {
            var carStatus = carStatusFilter != null
                ? _carStatusService.GetByPageWithConditions<CarStatusDto>(parameters, cs => cs.StatusName.ToLower().Contains(carStatusFilter.ToLower()))
                : _carStatusService.GetByPage<CarStatusDto>(parameters);

            if (carStatus == null || !carStatus.Any())
                return NoContent();

            ViewBag.CurrentPage = carStatus.MetaData.CurrentPage;
            ViewBag.PageSize = carStatus.MetaData.PageSize;
            ViewBag.TotalSize = carStatus.MetaData.TotalSize;

            ViewBag.CarStatusFilter = carStatusFilter ?? "";

            ViewBag.HaveNext = carStatus.MetaData.HaveNext;
            ViewBag.HavePrev = carStatus.MetaData.HavePrev;

            ViewBag.ControllerName = "CarStatus";
            ViewBag.ViewActionName = "cars-statuses";
            ViewBag.CreateActionName = "create-car-status-view";
            ViewBag.DeleteActionName = "delete-car-status";
            ViewBag.UpdateActionName = "update-car-status-view";

            return View(carStatus);
        }
        [HttpGet("create", Name = "create-car-status-view")]
        public IActionResult CreateView() => View();
        [HttpPost("create", Name = "create-car-status")]
        public async Task<IActionResult> Create([FromForm] CarStatusCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View("CreateView", dto);

            await _carStatusService.CreateAsync<CarStatusCreateDto, CarStatusDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete-settlement", Name = "delete-car-status")]
        public async Task<IActionResult> Delete([FromForm] CarStatusDeleteDto dto)
        {
            await _carStatusService.DeleteByIdAsync(dto.Id);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpGet("update", Name = "update-car-status-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var settlement = await _carStatusService.GetByIdAsync<CarStatusUpdateDto>(id);
            return View(settlement);
        }
        [HttpPost("update", Name = "update-car-status")]
        public async Task<IActionResult> Update([FromForm] CarStatusUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _carStatusService.UpdateAsync<CarStatusUpdateDto, CarStatusDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
