using Domain.Enums;

namespace Domain;

public class BaseManipulator
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public ManipulatorType Type { get; private set; }
    public string Position { get; private set; }

    protected BaseManipulator(Guid id, string name, ManipulatorType type, string position)
    {
        Id = id;
        Name = name;
        Type = type;
        Position = position;
    }

    public static BaseManipulator New(Guid id, string name, string position)
        => new(id, name, ManipulatorType.Base, position);
    
    public void UpdatePosition(string position) 
        => Position = position;
}