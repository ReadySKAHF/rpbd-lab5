using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using MVCApp.Controllers;

public class CarControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly Mock<IOwnerService> _mockOwnerService;
    private readonly CarController _controller;

    public CarControllerTests()
    {
        _mockCarService = new Mock<ICarService>();
        _mockOwnerService = new Mock<IOwnerService>();
        _controller = new CarController(_mockCarService.Object, _mockOwnerService.Object);
    }

    [Fact]
    public void Index_ReturnsViewWithCars_WhenCarsExist()
    {
        // Arrange
        var pagination = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var cars = new PagedList<CarDto>(
            new List<CarDto>
            {
                new CarDto { Id = Guid.NewGuid(), Brand = "MERCEDEZ", LicensePlate = "777RB" }
            },
            1, 10, 1);

        _mockCarService
            .Setup(service => service.GetByPage<CarDto>(pagination))
            .Returns(cars);

        // Act
        var result = _controller.Index(pagination, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CarDto>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public void Index_ReturnsNoContent_WhenNoCarsExist()
    {
        // Arrange
        var pagination = new PaginationQueryParameters { page = 1, pageSize = 10 };
        _mockCarService
            .Setup(service => service.GetByPage<CarDto>(pagination))
            .Returns((PagedList<CarDto>)null);

        // Act
        var result = _controller.Index(pagination, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void CreateView_ReturnsViewWithOwners()
    {
        // Arrange
        var owners = new List<OwnerDto>
        {
            new OwnerDto { Id = Guid.NewGuid(), FullName = "Zhenua drobysh" }
        };
        _mockOwnerService
            .Setup(service => service.GetAll<OwnerDto>())
            .Returns(owners);

        // Act
        var result = _controller.CreateView();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.ViewData["Owners"]);
        var selectList = Assert.IsType<SelectList>(viewResult.ViewData["Owners"]);
        Assert.Single(selectList);
    }

    [Fact]
    public async Task Create_RedirectsToIndex_WhenSuccessful()
    {
        // Arrange
        var dto = new CarCreateDto
        {
            LicensePlate = "QQ11",
            Brand = "BMW",
            Power = 150,
            Color = "Red",
            YearOfProduction = 2020,
            ChassisNumber = "CH123",
            EngineNumber = "EN456",
            DateReceived = DateTime.UtcNow,
            OwnerId = Guid.NewGuid()
        };

        _mockCarService
            .Setup(service => service.CreateAsync<CarCreateDto, CarDto>(dto))
            .ReturnsAsync(new CarDto { Id = Guid.NewGuid() });

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_ReturnsViewWithModel_WhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new CarCreateDto();
        _controller.ModelState.AddModelError("LicensePlate", "Required");

        var owners = new List<OwnerDto>
        {
            new OwnerDto { Id = Guid.NewGuid(), FullName = "John Doe" }
        };
        _mockOwnerService
            .Setup(service => service.GetAll<OwnerDto>())
            .Returns(owners);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateView", viewResult.ViewName);
        Assert.Same(dto, viewResult.Model);
    }

    [Fact]
    public async Task Delete_RedirectsToIndex_WhenSuccessful()
    {
        // Arrange
        var dto = new CarDeleteDto { Id = Guid.NewGuid() };

        _mockCarService
            .Setup(service => service.DeleteByIdAsync(dto.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task UpdateView_ReturnsViewWithCarAndOwners()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var car = new CarUpdateDto
        {
            Id = carId,
            LicensePlate = "ABC123",
            Brand = "Toyota",
            Power = 200
        };
        var owners = new List<OwnerDto>
        {
            new OwnerDto { Id = Guid.NewGuid(), FullName = "Zhenya" }
        };

        _mockCarService
            .Setup(service => service.GetByIdAsync<CarUpdateDto>(carId))
            .ReturnsAsync(car);
        _mockOwnerService
            .Setup(service => service.GetAll<OwnerDto>())
            .Returns(owners);

        // Act
        var result = await _controller.UpdateView(carId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(car, viewResult.Model);
        var selectList = Assert.IsType<SelectList>(viewResult.ViewData["Owners"]);
        Assert.Single(selectList);
    }
}
