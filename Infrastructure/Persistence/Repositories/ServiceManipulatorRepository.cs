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
        context.ServiceManipulators.Add(entity);
        context.SaveChanges();

        return entity;
    }

    public ServiceManipulator Remove(ServiceManipulator entity)
    {
        context.ServiceManipulators.Remove(entity);
        context.SaveChanges();

        return entity;
    }

    public ServiceManipulator UpdatePosition(Guid id, string newPosition)
    {
        var entity = context.ServiceManipulators
            .AsNoTracking()
            .First(m => m.Id == id);
        entity.UpdatePosition(newPosition);
        context.Update(entity);
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
        context.Update(manipulator);
        context.SaveChanges();
        return manipulator;
    }
}