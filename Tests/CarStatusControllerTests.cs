using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;

public class CarStatusControllerTests
{
    private readonly Mock<ICarStatusService> _mockCarStatusService;
    private readonly CarStatusController _controller;

    public CarStatusControllerTests()
    {
        _mockCarStatusService = new Mock<ICarStatusService>();
        _controller = new CarStatusController(_mockCarStatusService.Object);
    }

    [Fact]
    public void IndexReturnsViewResultWithCarStatuses()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var carStatuses = new PagedList<CarStatusDto>(
            new List<CarStatusDto>
            {
                new CarStatusDto { Id = Guid.NewGuid(), StatusName = "Available" },
                new CarStatusDto { Id = Guid.NewGuid(), StatusName = "Unavailable" }
            },
            2, 1, 10);

        _mockCarStatusService
            .Setup(service => service.GetByPage<CarStatusDto>(paginationParams))
            .Returns(carStatuses);

        // Act
        var result = _controller.Index(paginationParams, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CarStatusDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void IndexReturnsNoContentWhenNoCarStatuses()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var carStatuses = new PagedList<CarStatusDto>(new List<CarStatusDto>(), 0, 1, 10);

        _mockCarStatusService
            .Setup(service => service.GetByPage<CarStatusDto>(paginationParams))
            .Returns(carStatuses);

        // Act
        var result = _controller.Index(paginationParams, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void CreateViewReturnsViewResult()
    {
        // Act
        var result = _controller.CreateView();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreateRedirectsToIndexWhenSuccessful()
    {
        // Arrange
        var dto = new CarStatusCreateDto { StatusName = "Available" };

        _mockCarStatusService
            .Setup(service => service.CreateAsync<CarStatusCreateDto, CarStatusDto>(dto))
            .ReturnsAsync(new CarStatusDto
            {
                Id = Guid.NewGuid(),
                StatusName = dto.StatusName
            });


        // Act
        var result = await _controller.Create(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task CreateReturnsCreateViewWhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new CarStatusCreateDto { StatusName = "" };
        _controller.ModelState.AddModelError("StatusName", "Status Name is required.");

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateView", viewResult.ViewName);
    }

    [Fact]
    public async Task DeleteRedirectsToIndexWhenSuccessful()
    {
        // Arrange
        var dto = new CarStatusDeleteDto { Id = Guid.NewGuid() };

        _mockCarStatusService
            .Setup(service => service.DeleteByIdAsync(dto.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task UpdateViewReturnsViewWithCarStatus()
    {
        // Arrange
        var id = Guid.NewGuid();
        var carStatus = new CarStatusUpdateDto { Id = id, StatusName = "Available" };

        _mockCarStatusService
            .Setup(service => service.GetByIdAsync<CarStatusUpdateDto>(id))
            .ReturnsAsync(carStatus);

        // Act
        var result = await _controller.UpdateView(id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CarStatusUpdateDto>(viewResult.Model);
        Assert.Equal(id, model.Id);
    }

    [Fact]
    public async Task UpdateRedirectsToIndexWhenSuccessful()
    {
        // Arrange
        var dto = new CarStatusUpdateDto { Id = Guid.NewGuid(), StatusName = "Available" };

        _mockCarStatusService
            .Setup(service => service.UpdateAsync<CarStatusUpdateDto, CarStatusDto>(dto))
            .ReturnsAsync(new CarStatusDto
            {
                Id = dto.Id,
                StatusName = dto.StatusName
            });


        // Act
        var result = await _controller.Update(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task UpdateReturnsUpdateViewWhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new CarStatusUpdateDto { Id = null, StatusName = "" };
        _controller.ModelState.AddModelError("StatusName", "Status Name is required.");

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateView", viewResult.ViewName);
    }
}
