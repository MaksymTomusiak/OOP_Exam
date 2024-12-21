using Domain.Enums;

namespace Domain;

public class IndustrialManipulator : BaseManipulator
{
    public int WeldsAmount { get; private set; }
    
    protected IndustrialManipulator(Guid id, string name, ManipulatorType type, string position)
        : base(id, name, type, position)
    {
        WeldsAmount = 0;
    }
    
    public new static IndustrialManipulator New(Guid id, string name, string position)
        => new(id, name, ManipulatorType.Industrial, position);

    public void Weld()
    {
        WeldsAmount++;
    }
}