using Application.Common.Interfaces;
using Application.Managers;
using Application.ManipulatorFactories;
using Domain;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common;
using Xunit;

namespace Tests.Tests.Integrational;

public class ManipulatorManagerTests : BaseTest, IAsyncLifetime
{
    private readonly IManipulatorManager _manager;
    private readonly IndustrialManipulatorFactory _industrialIndustrialManipulatorFactory;
    private readonly ServiceManipulatorFactory _serviceManipulatorFactory;
    public ManipulatorManagerTests(TestFactory factory) : base(factory)
    {
        _manager = factory.ServiceProvider.GetRequiredService<IManipulatorManager>();
        using var scope = factory.ServiceProvider.CreateScope();
        _industrialIndustrialManipulatorFactory = scope.ServiceProvider.GetRequiredService<IndustrialManipulatorFactory>();
        _serviceManipulatorFactory = scope.ServiceProvider.GetRequiredService<ServiceManipulatorFactory>();
    }


    [Fact]
    public void ShouldAddIndustrialManipulator()
    {
        // Arrange
        var manipulator = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial name", "industrial warehouse");

        // Act
        var success = _manager.AddManipulator(manipulator);

        // Assert
        success.Should().BeTrue();

        var dbManipulator = Context.IndustrialManipulators.FirstOrDefault(m => m.Id == manipulator.Id);
        dbManipulator.Should().NotBeNull();
        dbManipulator!.Name.Should().Be(manipulator.Name);
        dbManipulator.Position.Should().Be(manipulator.Position);
        dbManipulator.WeldsAmount.Should().Be(0);
    }

    [Fact]
    public void ShouldNotAddIndustrialManipulatorBecauseNullPassed()
    {
        // Arrange
        IndustrialManipulator manipulator = null;

        // Act
        var success = _manager.AddManipulator(manipulator);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void ShouldAddServiceManipulator()
    {
        // Arrange
        var manipulator = _serviceManipulatorFactory.CreateManipulator("Service name", "service warehouse");

        // Act
        var success = _manager.AddManipulator(manipulator);

        // Assert
        success.Should().BeTrue();

        var dbManipulator = Context.ServiceManipulators.FirstOrDefault(m => m.Id == manipulator.Id);
        dbManipulator.Should().NotBeNull();
        dbManipulator!.Name.Should().Be(manipulator.Name);
        dbManipulator.Position.Should().Be(manipulator.Position);
        dbManipulator.ServesAmount.Should().Be(0);
    }

    [Fact]
    public void ShouldRemoveIndustrialManipulator()
    {
        // Arrange
        var manipulator = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial name", "industrial warehouse");
        _manager.AddManipulator(manipulator);

        // Act
        var success = _manager.RemoveManipulator<IndustrialManipulator>(manipulator.Id);

        // Assert
        success.Should().BeTrue();
        var dbManipulator = Context.IndustrialManipulators.FirstOrDefault(m => m.Id == manipulator.Id);
        dbManipulator.Should().BeNull();
    }

    [Fact]
    public void ShouldNotRemoveManipulatorBecauseIdNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var success = _manager.RemoveManipulator<IndustrialManipulator>(invalidId);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void ShouldGetIndustrialManipulatorById()
    {
        // Arrange
        var manipulator = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial name", "industrial warehouse");
        _manager.AddManipulator(manipulator);

        // Act
        var dbManipulator = _manager.GetManipulator<IndustrialManipulator>(manipulator.Id);

        // Assert
        dbManipulator.Should().NotBeNull();
        dbManipulator!.Name.Should().Be(manipulator.Name);
        dbManipulator.Position.Should().Be(manipulator.Position);
    }

    [Fact]
    public void ShouldReturnNullForNonExistentManipulator()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var dbManipulator = _manager.GetManipulator<IndustrialManipulator>(invalidId);

        // Assert
        dbManipulator.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnAllIndustrialManipulators()
    {
        // Arrange
        var manipulator1 = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial 1", "warehouse 1");
        var manipulator2 = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial 2", "warehouse 2");
        _manager.AddManipulator(manipulator1);
        _manager.AddManipulator(manipulator2);

        // Act
        var allManipulators = _manager.GetAllManipulators<IndustrialManipulator>();

        // Assert
        allManipulators.Should().NotBeNullOrEmpty();
        allManipulators.Should().HaveCount(2);
        allManipulators.Should().Contain(m => m.Id == manipulator1.Id);
        allManipulators.Should().Contain(m => m.Id == manipulator2.Id);
    }

    [Fact]
    public void ShouldReturnEmptyListForNoIndustrialManipulators()
    {
        // Act
        var allManipulators = _manager.GetAllManipulators<IndustrialManipulator>();

        // Assert
        allManipulators.Should().BeEmpty();
    }

    [Fact]
    public void ShouldUpdateManipulatorPosition()
    {
        // Arrange
        var manipulator = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial name", "initial warehouse");
        _manager.AddManipulator(manipulator);
        var newPosition = "updated warehouse";

        // Act
        var success = _manager.UpdateManipulatorPosition(manipulator.Id, newPosition);

        // Assert
        success.Should().BeTrue();
        var dbManipulator = Context.IndustrialManipulators.First(m => m.Id == manipulator.Id);
        dbManipulator.Position.Should().Be(newPosition);
    }

    [Fact]
    public void ShouldNotUpdateManipulatorPositionForInvalidId()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var newPosition = "updated warehouse";

        // Act
        var success = _manager.UpdateManipulatorPosition(invalidId, newPosition);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void ShouldWeldIndustrialManipulator()
    {
        // Arrange
        var manipulator = _industrialIndustrialManipulatorFactory.CreateManipulator("Industrial name", "warehouse");
        _manager.AddManipulator(manipulator);

        // Act
        var success = _manager.Weld((IndustrialManipulator)manipulator);

        // Assert
        success.Should().BeTrue();
        var dbManipulator = Context.IndustrialManipulators.First(m => m.Id == manipulator.Id);
        dbManipulator.WeldsAmount.Should().Be(1);
    }

    [Fact]
    public void ShouldNotWeldInvalidIndustrialManipulator()
    {
        // Arrange
        IndustrialManipulator manipulator = null;

        // Act
        var success = _manager.Weld(manipulator);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void ShouldServeServiceManipulator()
    {
        // Arrange
        var manipulator = _serviceManipulatorFactory.CreateManipulator("Service name", "warehouse");
        _manager.AddManipulator(manipulator);

        // Act
        var success = _manager.Serve((ServiceManipulator)manipulator);

        // Assert
        success.Should().BeTrue();
        var dbManipulator = Context.ServiceManipulators.First(m => m.Id == manipulator.Id);
        dbManipulator.ServesAmount.Should().Be(1);
    }

    [Fact]
    public void ShouldNotServeInvalidServiceManipulator()
    {
        // Arrange
        ServiceManipulator manipulator = null;

        // Act
        var success = _manager.Serve(manipulator);

        // Assert
        success.Should().BeFalse();
    }


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Context.BaseManipulators.RemoveRange(Context.BaseManipulators);
        Context.ServiceManipulators.RemoveRange(Context.ServiceManipulators);
        Context.IndustrialManipulators.RemoveRange(Context.IndustrialManipulators);
        await SaveChangesAsync();
    }
}