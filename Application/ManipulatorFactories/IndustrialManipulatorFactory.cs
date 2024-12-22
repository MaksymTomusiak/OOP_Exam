using Domain;

namespace Application.ManipulatorFactories;

public class IndustrialManipulatorFactory : IManipulatorFactory
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
        return IndustrialManipulator.New(Guid.NewGuid(), name, position);
    }
}