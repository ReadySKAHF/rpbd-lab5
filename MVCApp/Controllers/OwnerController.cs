using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin", "User")]
    [Route("owner")]
    [ApiController]
    public class OwnerController : BaseController
    {
        private readonly IOwnerService _ownerService;
        private readonly ICarService _carService;

        public OwnerController(IOwnerService routeService, ICarService settlementService)
        {
            _ownerService = routeService;
            _carService = settlementService;
        }

        [HttpGet("", Name = "owners")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters, string? fullNameFilter)
        {
            var owners = fullNameFilter != null 
                ? _ownerService.GetByPageWithConditions<OwnerDto>(parameters, o => o.FullName.ToLower().Contains(fullNameFilter.ToLower()))
                : _ownerService.GetByPage<OwnerDto>(parameters);

            if (owners == null || !owners.Any())
                return NoContent();

            ViewBag.CurrentPage = owners.MetaData.CurrentPage;
            ViewBag.PageSize = owners.MetaData.PageSize;
            ViewBag.TotalSize = owners.MetaData.TotalSize;

            ViewBag.FullNameFilter = fullNameFilter ?? "";

            ViewBag.HaveNext = owners.MetaData.HaveNext;
            ViewBag.HavePrev = owners.MetaData.HavePrev;

            ViewBag.ControllerName = "Owner";
            ViewBag.ViewActionName = "owners";
            ViewBag.CreateActionName = "create-owner-view";
            ViewBag.DeleteActionName = "delete-owner";
            ViewBag.UpdateActionName = "update-owner-view";

            return View(owners);
        }
        [HttpGet("create", Name = "create-owner-view")]
        public IActionResult CreateView() => View();
        [HttpPost("", Name = "create-owner")]
        public async Task<IActionResult> Create([FromForm] OwnerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _ownerService.CreateAsync<OwnerCreateDto, OwnerDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete-owner", Name = "delete-owner")]
        public async Task<IActionResult> Delete([FromForm] OwnerDeleteDto dto)
        {
            await _ownerService.DeleteByIdAsync(dto.Id);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpGet("update", Name = "update-owner-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var route = await _ownerService.GetByIdAsync<OwnerUpdateDto>(id);
            return View(route);
        }
        [HttpPost("update", Name = "update-owner")]
        public async Task<IActionResult> Update([FromForm] OwnerUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _ownerService.UpdateAsync<OwnerUpdateDto, OwnerDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
