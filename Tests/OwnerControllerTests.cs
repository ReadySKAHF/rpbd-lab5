using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;

public class OwnerControllerTests
{
    private readonly Mock<IOwnerService> _mockOwnerService;
    private readonly Mock<ICarService> _mockCarService;
    private readonly OwnerController _controller;

    public OwnerControllerTests()
    {
        _mockOwnerService = new Mock<IOwnerService>();
        _mockCarService = new Mock<ICarService>();
        _controller = new OwnerController(_mockOwnerService.Object, _mockCarService.Object);
    }

    [Fact]
    public void Index_ReturnsViewWithOwners_WhenOwnersExist()
    {
        // Arrange
        var pagination = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var owners = new PagedList<OwnerDto>(
            new List<OwnerDto>
            {
                new OwnerDto { Id = Guid.NewGuid(), FullName = "Zhenya", DriverLicenseNumber = "RB777" }
            },
            1, 10, 1);

        _mockOwnerService
            .Setup(service => service.GetByPage<OwnerDto>(pagination))
            .Returns(owners);

        // Act
        var result = _controller.Index(pagination, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<OwnerDto>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public void Index_ReturnsNoContent_WhenNoOwnersExist()
    {
        // Arrange
        var pagination = new PaginationQueryParameters { page = 1, pageSize = 10 };
        _mockOwnerService
            .Setup(service => service.GetByPage<OwnerDto>(pagination))
            .Returns((PagedList<OwnerDto>)null);

        // Act
        var result = _controller.Index(pagination, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void CreateView_ReturnsView()
    {
        // Act
        var result = _controller.CreateView();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_RedirectsToIndex_WhenSuccessful()
    {
        // Arrange
        var dto = new OwnerCreateDto
        {
            DriverLicenseNumber = "RB777",
            FullName = "Zhenya",
            Address = "123 Street",
            Phone = "1234567890"
        };

        _mockOwnerService
            .Setup(service => service.CreateAsync<OwnerCreateDto, OwnerDto>(dto))
            .ReturnsAsync(new OwnerDto { Id = Guid.NewGuid() });

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
        var dto = new OwnerCreateDto();
        _controller.ModelState.AddModelError("DriverLicenseNumber", "Required");

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Same(dto, viewResult.Model);
    }

    [Fact]
    public async Task Delete_RedirectsToIndex_WhenSuccessful()
    {
        // Arrange
        var dto = new OwnerDeleteDto { Id = Guid.NewGuid() };

        _mockOwnerService
            .Setup(service => service.DeleteByIdAsync(dto.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task UpdateView_ReturnsViewWithOwner_WhenOwnerExists()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var owner = new OwnerUpdateDto
        {
            Id = ownerId,
            DriverLicenseNumber = "RB777",
            FullName = "Zhenya",
            Address = "123 Street",
            Phone = "1234567890"
        };

        _mockOwnerService
            .Setup(service => service.GetByIdAsync<OwnerUpdateDto>(ownerId))
            .ReturnsAsync(owner);

        // Act
        var result = await _controller.UpdateView(ownerId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(owner, viewResult.Model);
    }

    [Fact]
    public async Task Update_RedirectsToIndex_WhenSuccessful()
    {
        // Arrange
        var dto = new OwnerUpdateDto
        {
            Id = Guid.NewGuid(),
            DriverLicenseNumber = "RB777",
            FullName = "Zhenya",
            Address = "123 Street",
            Phone = "1234567890"
        };

        _mockOwnerService
            .Setup(service => service.UpdateAsync<OwnerUpdateDto, OwnerDto>(dto))
            .ReturnsAsync(new OwnerDto());

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Update_ReturnsViewWithModel_WhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new OwnerUpdateDto();
        _controller.ModelState.AddModelError("DriverLicenseNumber", "Required");

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateView", viewResult.ViewName);
        Assert.Same(dto, viewResult.Model);
    }
}
