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
        context.IndustrialManipulators.Add(entity);
        context.SaveChanges();

        return entity;
    }

    public IndustrialManipulator Remove(IndustrialManipulator entity)
    {
        context.IndustrialManipulators.Remove(entity);
        context.SaveChanges();

        return entity;
    }

    public IndustrialManipulator UpdatePosition(Guid id, string newPosition)
    {
        var entity = context.IndustrialManipulators
            .AsNoTracking()
            .First(m => m.Id == id);
        entity.UpdatePosition(newPosition);
        context.Update(entity);
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
        context.Update(manipulator);
        context.SaveChanges();
        return manipulator;
    }
}