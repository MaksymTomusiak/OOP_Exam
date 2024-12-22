using Domain;

namespace Application.ManipulatorFactories;

public class ServiceManipulatorFactory : IManipulatorFactory
{
    public BaseManipulator CreateManipulator(string name, string position)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException("Name should not be empty");
        }
        if (string.IsNullOrEmpty(position))
        {
            throw new ArgumentNullException("Position should not be empty");
        }
        return ServiceManipulator.New(Guid.NewGuid(), name, position);
    }
}