using Domain;

namespace Application.Common.Interfaces;

public interface IRepository<T>  where T : class
{
    T Get(Guid id);
    T  Add(T entity);
    T Remove(T entity);
    IEnumerable<T> GetAll();
    T Update(T manipulator);
}