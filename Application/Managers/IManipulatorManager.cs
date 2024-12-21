using Domain;

namespace Application.Managers;

public interface IManipulatorManager
{
    bool AddManipulator<T>(T manipulator) where T : BaseManipulator;
    bool RemoveManipulator<T>(Guid manipulatorId) where T : BaseManipulator;
    IEnumerable<T> GetAllManipulators<T>() where T : BaseManipulator;
    T? GetManipulator<T>(Guid manipulatorId)  where T : BaseManipulator;
    bool UpdateManipulatorPosition<T>(Guid manipulatorId, string newPosition) where T : BaseManipulator;
    bool Weld(IndustrialManipulator manipulator);
    bool Serve(ServiceManipulator manipulator);
}