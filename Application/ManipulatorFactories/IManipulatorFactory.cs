using Domain;

namespace Application.ManipulatorFactories;

public interface IManipulatorFactory
{
    BaseManipulator CreateManipulator(string name, string position);
}