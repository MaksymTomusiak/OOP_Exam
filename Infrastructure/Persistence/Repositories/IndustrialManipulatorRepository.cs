using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class IndustrialManipulatorRepository(ApplicationDbContext context) : IRepository<IndustrialManipulator>
{
    public IndustrialManipulator Get(Guid id)
    {
        return context.IndustrialManipulators
            .AsNoTracking()
            .First(m => m.Id == id);
    }

    public IndustrialManipulator Add(IndustrialManipulator entity)
    {
        var existingEntity = context.IndustrialManipulators.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.IndustrialManipulators.Add(entity);
        context.SaveChanges();
        return entity;
    }

    public IndustrialManipulator Remove(IndustrialManipulator entity)
    {
        var existingEntity = context.IndustrialManipulators.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.IndustrialManipulators.Remove(entity);
        context.SaveChanges();
        return entity;
    }

    public IEnumerable<IndustrialManipulator> GetAll()
    {
        return context.IndustrialManipulators
            .AsNoTracking()
            .ToList();
    }

    public IndustrialManipulator Update(IndustrialManipulator manipulator)
    {
        var existingEntity = context.IndustrialManipulators.Local.FirstOrDefault(e => e.Id == manipulator.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.Update(manipulator);
        context.SaveChanges();
        return manipulator;
    }
}
