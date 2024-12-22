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
        var existingEntity = context.BaseManipulators.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.BaseManipulators.Add(entity);
        context.SaveChanges();

        return entity;
    }

    public BaseManipulator Remove(BaseManipulator entity)
    {
        var existingEntity = context.BaseManipulators.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.BaseManipulators.Remove(entity);
        context.SaveChanges();

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
        var existingEntity = context.BaseManipulators.Local.FirstOrDefault(e => e.Id == manipulator.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.Update(manipulator);
        context.SaveChanges();

        return manipulator;
    }
}