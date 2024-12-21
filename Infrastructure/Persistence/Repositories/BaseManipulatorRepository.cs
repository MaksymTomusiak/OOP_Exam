using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BaseManipulatorRepository(ApplicationDbContext context) : IRepository<BaseManipulator>
{
    public BaseManipulator Get(Guid id)
    {
        return context.BaseManipulators
            .AsNoTracking()
            .First(m => m.Id == id);
    }

    public BaseManipulator Add(BaseManipulator entity)
    {
        context.BaseManipulators.Add(entity);
        context.SaveChanges();

        return entity;
    }

    public BaseManipulator Remove(BaseManipulator entity)
    {
        context.BaseManipulators.Remove(entity);
        context.SaveChanges();

        return entity;
    }

    public BaseManipulator UpdatePosition(Guid id, string newPosition)
    {
        var entity = context.BaseManipulators
            .AsNoTracking()
            .First(m => m.Id == id);
        entity.UpdatePosition(newPosition);
        context.Update(entity);
        return entity;
    }

    public IEnumerable<BaseManipulator> GetAll()
    {
        return context.BaseManipulators
            .AsNoTracking()
            .ToList();
    }

    public BaseManipulator Update(BaseManipulator manipulator)
    {
        context.Update(manipulator);
        context.SaveChanges();
        return manipulator;
    }
}