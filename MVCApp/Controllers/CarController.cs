using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin", "User")]
    [Route("car")]
    [ApiController]
    public class CarController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IOwnerService _ownerService;

        public CarController(ICarService carService, IOwnerService ownerService)
        {
            _carService = carService;
            _ownerService = ownerService;

        }

        [HttpGet("", Name = "cars")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters, string? brandFilter)
        {
            var cars = brandFilter != null
                ? _carService.GetByPageWithConditions<CarDto>(parameters, c => c.Brand.ToLower().Contains(brandFilter.ToLower()))
                : _carService.GetByPage<CarDto>(parameters);

            if (cars == null || !cars.Any())
                return NoContent();

            ViewBag.CurrentPage = cars.MetaData.CurrentPage;
            ViewBag.PageSize = cars.MetaData.PageSize;
            ViewBag.TotalSize = cars.MetaData.TotalSize;

            ViewBag.BrandFilter = brandFilter ?? "";

            ViewBag.HaveNext = cars.MetaData.HaveNext;
            ViewBag.HavePrev = cars.MetaData.HavePrev;

            ViewBag.ControllerName = "Car";
            ViewBag.ViewActionName = "cars";
            ViewBag.CreateActionName = "create-car-view";
            ViewBag.DeleteActionName = "delete-car";
            ViewBag.UpdateActionName = "update-car-view";


            return View(cars);
        }
        [HttpGet("create", Name = "create-car-view")]
        public IActionResult CreateView()
        {
            var owners = _ownerService.GetAll<OwnerDto>();
            ViewBag.Owners = new SelectList(owners, "Id", "FullName");
            return View();

        }
        [HttpPost("", Name = "create-car")]
        public async Task<IActionResult> Create([FromForm] CarCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var owners = _ownerService.GetAll<OwnerDto>();
                ViewBag.Owners = new SelectList(owners, "Id", "FullName");
                return View("CreateView", dto);
            }
            await _carService.CreateAsync<CarCreateDto, CarDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete-car", Name = "delete-car")]
        public async Task<IActionResult> Delete([FromForm] CarDeleteDto dto)
        {
            await _carService.DeleteByIdAsync(dto.Id);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpGet("update", Name = "update-car-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var owners = _ownerService.GetAll<OwnerDto>();
            ViewBag.Owners = new SelectList(owners, "Id", "FullName");

            var cargo = await _carService.GetByIdAsync<CarUpdateDto>(id);
            return View(cargo);
        }
        [HttpPost("update", Name = "update-car")]
        public async Task<IActionResult> Update([FromForm] CarUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _carService.UpdateAsync<CarUpdateDto, CarDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
