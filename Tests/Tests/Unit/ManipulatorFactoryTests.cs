using Application.ManipulatorFactories;
using FluentAssertions;
using Xunit;

namespace Tests.Tests.Unit;

public class ManipulatorFactoryTests
{
    private readonly IndustrialManipulatorFactory _industrialIndustrialManipulatorFactory;
    private readonly ServiceManipulatorFactory _serviceManipulatorFactory;

    public ManipulatorFactoryTests()
    {
        _industrialIndustrialManipulatorFactory = new IndustrialManipulatorFactory();
        _serviceManipulatorFactory = new ServiceManipulatorFactory();
    }

    [Fact]
    public void ShouldCreateIndustrialManipulator()
    {
        // Arrange
        var name = "Industrial name";
        var position = "industrial warehouse";

        // Act
        var manipulator = _industrialIndustrialManipulatorFactory.CreateManipulator(name, position);

        // Assert
        manipulator.Id.Should().NotBeEmpty();
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
    }

    [Fact]
    public void ShouldCreateServiceManipulator()
    {
        // Arrange
        var name = "Service name";
        var position = "service warehouse";

        // Act
        var manipulator = _serviceManipulatorFactory.CreateManipulator(name, position);

        // Assert
        manipulator.Id.Should().NotBeEmpty();
        manipulator.Name.Should().Be(name);
        manipulator.Position.Should().Be(position);
    }

    [Fact]
    public void ShouldThrowExceptionWhenCreatingIndustrialManipulatorWithNullName()
    {
        // Arrange
        string name = null;
        var position = "industrial warehouse";

        // Act
        var act = () => _industrialIndustrialManipulatorFactory.CreateManipulator(name, position);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*Name should not be empty*");
    }

    [Fact]
    public void ShouldThrowExceptionWhenCreatingIndustrialManipulatorWithNullPosition()
    {
        // Arrange
        var name = "Industrial name";
        string position = null;

        // Act
        var act = () => _industrialIndustrialManipulatorFactory.CreateManipulator(name, position);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*Position should not be empty*");
    }

    [Fact]
    public void ShouldThrowExceptionWhenCreatingServiceManipulatorWithNullName()
    {
        // Arrange
        string name = null;
        var position = "service warehouse";

        // Act
        var act = () => _serviceManipulatorFactory.CreateManipulator(name, position);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*Name should not be empty*");
    }

    [Fact]
    public void ShouldThrowExceptionWhenCreatingServiceManipulatorWithNullPosition()
    {
        // Arrange
        var name = "Service name";
        string position = null;

        // Act
        var act = () => _serviceManipulatorFactory.CreateManipulator(name, position);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*Position should not be empty*");
    }
}
