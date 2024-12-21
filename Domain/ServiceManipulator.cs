using Domain.Enums;

namespace Domain;

public class ServiceManipulator : BaseManipulator
{
    public int ServesAmount { get; private set; }
    
    protected ServiceManipulator(Guid id, string name, ManipulatorType type, string position) : base(id, name, type, position)
    {
        ServesAmount = 0;
    }
    
    public new static ServiceManipulator New(Guid id, string name, string position) 
        => new(id, name, ManipulatorType.Service, position);

    public void Serve()
    {
        ServesAmount++;
    }
}