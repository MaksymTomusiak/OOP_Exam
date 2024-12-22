using Domain;

namespace Application.Managers;

public interface IManipulatorManager: IEnumerable<BaseManipulator>
{
    bool AddManipulator(BaseManipulator manipulator);
    bool RemoveManipulator<T>(Guid manipulatorId) where T : BaseManipulator;
    IEnumerable<T> GetAllManipulators<T>() where T : BaseManipulator;
    T? GetManipulator<T>(Guid manipulatorId)  where T : BaseManipulator;
    bool UpdateManipulatorPosition(Guid manipulatorId, string newPosition);
    bool Weld(IndustrialManipulator manipulator);
    bool Serve(ServiceManipulator manipulator);
}