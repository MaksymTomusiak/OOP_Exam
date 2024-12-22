using Domain;
using FluentAssertions;
using Tests.Common;
using Xunit;

namespace Tests.Tests.Unit;

public class ManipulatorsTests
{
    [Fact]
    public void ShouldCreateBaseManipulator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Base name";
        var position = "base warehouse";

        // Act
        var manipulator = BaseManipulator.New(id, name, position);

        // Assert
        manipulator.Id.Should().Be(id);
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
    }

    [Fact]
    public void ShouldCreateServiceManipulator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Service name";
        var position = "service warehouse";

        // Act
        var manipulator = ServiceManipulator.New(id, name, position);

        // Assert
        manipulator.Id.Should().Be(id);
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
        manipulator.ServesAmount.Should().Be(0);
    }

    [Fact]
    public void ShouldCreateIndustrialManipulator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Industrial name";
        var position = "industrial warehouse";

        // Act
        var manipulator = IndustrialManipulator.New(id, name, position);

        // Assert
        manipulator.Id.Should().Be(id);
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
        manipulator.WeldsAmount.Should().Be(0);
    }

    [Fact]
    public void ShouldPerformWeld()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Industrial name";
        var position = "industrial warehouse";

        // Act
        var manipulator = IndustrialManipulator.New(id, name, position);
        manipulator.Weld();

        // Assert
        manipulator.Id.Should().Be(id);
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
        manipulator.WeldsAmount.Should().Be(1);
    }

    [Fact]
    public void ShouldPerformServe()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Service name";
        var position = "service warehouse";

        // Act
        var manipulator = ServiceManipulator.New(id, name, position);
        manipulator.Serve();

        // Assert
        manipulator.Id.Should().Be(id);
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
        manipulator.ServesAmount.Should().Be(1);
    }
}