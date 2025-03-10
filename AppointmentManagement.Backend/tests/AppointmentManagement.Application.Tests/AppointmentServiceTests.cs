using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Application.Services;
using AppointmentManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Domain.Enums;

namespace AppointmentManagement.Application.Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IUserAppointmentRepository> _userAppointmentRepositoryMock;
    private readonly Mock<ILogger<AppointmentService>> _loggerMock;
    private readonly AppointmentService _appointmentService;

    public AppointmentServiceTests()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _userAppointmentRepositoryMock = new Mock<IUserAppointmentRepository>();
        _loggerMock = new Mock<ILogger<AppointmentService>>();
        _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object, _userAppointmentRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ApproveAppointmentAsync_Should_Return_Success_When_Appointment_Exists_And_Not_Canceled()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, Status = AppointmentStatus.Pending };
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(appointment);
        _appointmentRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Appointment>())).ReturnsAsync(new Appointment
        {
            Id = 1,
            Status = AppointmentStatus.Approved,
        });

        // Act
        var response = await _appointmentService.ApproveAppointmentAsync(1);

        // Assert
        Assert.True(response.Success);
        Assert.True(response.Data);
        _appointmentRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Appointment>(a => a.Status == AppointmentStatus.Approved)), Times.Once);
    }

    [Fact]
    public async Task ApproveAppointmentAsync_Should_Return_Error_When_Appointment_Does_Not_Exist()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Appointment)null);

        // Act
        var response = await _appointmentService.ApproveAppointmentAsync(1);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Cannot approve a non-existent or canceled appointment.", response.Message);
    }

    [Fact]
    public async Task ApproveAppointmentAsync_Should_Return_Error_When_Appointment_Is_Canceled()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, Status = AppointmentStatus.Canceled };
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(appointment);

        // Act
        var response = await _appointmentService.ApproveAppointmentAsync(1);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Cannot approve a non-existent or canceled appointment.", response.Message);
    }
}