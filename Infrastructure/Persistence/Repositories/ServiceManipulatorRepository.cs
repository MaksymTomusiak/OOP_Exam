using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ServiceManipulatorRepository(ApplicationDbContext context) : IRepository<ServiceManipulator>
{
    public ServiceManipulator Get(Guid id)
    {
        return context.ServiceManipulators
            .AsNoTracking()
            .First(m => m.Id == id);
    }

    public ServiceManipulator Add(ServiceManipulator entity)
    {
        var existingEntity = context.ServiceManipulators.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.ServiceManipulators.Add(entity);
        context.SaveChanges();
        return entity;
    }

    public ServiceManipulator Remove(ServiceManipulator entity)
    {
        var existingEntity = context.ServiceManipulators.Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.ServiceManipulators.Remove(entity);
        context.SaveChanges();
        return entity;
    }

    public IEnumerable<ServiceManipulator> GetAll()
    {
        return context.ServiceManipulators
            .AsNoTracking()
            .ToList();
    }

    public ServiceManipulator Update(ServiceManipulator manipulator)
    {
        var existingEntity = context.ServiceManipulators.Local.FirstOrDefault(e => e.Id == manipulator.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }
        context.Update(manipulator);
        context.SaveChanges();
        return manipulator;
    }
}