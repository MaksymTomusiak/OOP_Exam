using System.Collections;
using Application.Common.Interfaces;
using Domain;
using Application.Loggers;

namespace Application.Managers
{
    public class ManipulatorManager(
        IRepository<BaseManipulator> baseManipulatorRepository,
        IRepository<ServiceManipulator> serviceManipulatorRepository,
        IRepository<IndustrialManipulator> industrialManipulatorRepository,
            ILogger logger)
        : IManipulatorManager, IEnumerable<BaseManipulator>
    {
        private readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>
        {
            { typeof(BaseManipulator), baseManipulatorRepository },
            { typeof(ServiceManipulator), serviceManipulatorRepository },
            { typeof(IndustrialManipulator), industrialManipulatorRepository }
        };

        private IRepository<T> GetRepository<T>() where T : BaseManipulator
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }
            logger.LogError(new InvalidOperationException($"Repository not found for type {typeof(T)}"), "Repository not found.");
            throw new InvalidOperationException($"Repository not found for type {typeof(T)}");
        }

        public bool AddManipulator<T>(T manipulator) where T : BaseManipulator
        {
            try
            {
                var repository = GetRepository<T>();
                repository.Add(manipulator);
                logger.LogInfo($"Added manipulator of type {manipulator.GetType().Name} with id {manipulator.Id} and name {manipulator.Name} to the database.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding manipulator to the database.");
                return false;
            }
        }

        public bool RemoveManipulator<T>(Guid manipulatorId) where T : BaseManipulator
        {
            try
            {
                var repository = GetRepository<T>();
                var manipulator = repository.Get(manipulatorId);
                repository.Remove(manipulator);
                logger.LogInfo($"Removed manipulator of type {manipulator.GetType().Name} with id {manipulator.Id} and name {manipulator.Name} to the database.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing manipulator from the database.");
                return false;
            }
        }

        public IEnumerable<T> GetAllManipulators<T>() where T : BaseManipulator
        {
            try
            {
                var repository = GetRepository<T>();
                return repository.GetAll();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting manipulators from the database.");
                return Enumerable.Empty<T>();
            }
        }

        public T? GetManipulator<T>(Guid manipulatorId) where T : BaseManipulator
        {
            try
            {
                var repository = GetRepository<T>();
                return repository.Get(manipulatorId);
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting manipulators from the database by id {manipulatorId}.");
                return null;
            }
        }

        public bool UpdateManipulatorPosition<T>(Guid manipulatorId, string newPosition) where T : BaseManipulator
        {
            try
            {
                var repository = GetRepository<T>();
                repository.UpdatePosition(manipulatorId, newPosition);
                logger.LogInfo($"Updated position of manipulator under id {manipulatorId} to {newPosition} in the database.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating position of manipulator under id {manipulatorId} to {newPosition} in the database.");
                return false;
            }
        }

        public bool Weld(IndustrialManipulator manipulator)
        {
            try
            {
                var repository = GetRepository<IndustrialManipulator>();
                manipulator.Weld();
                repository.Update(manipulator);
                logger.LogInfo($"Manipulator under id {manipulator.Id} performed welding.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Manipulator under id {manipulator.Id} couldn't perform welding.");
                return false;
            }
        }

        public bool Serve(ServiceManipulator manipulator)
        {
            try
            {
                var repository = GetRepository<ServiceManipulator>();
                manipulator.Serve();
                repository.Update(manipulator);
                logger.LogInfo($"Manipulator under id {manipulator.Id} performed serving.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Manipulator under id {manipulator.Id} couldn't perform serving.");
                return false;
            }
        }
        
        public IEnumerator<BaseManipulator> GetEnumerator()
        {
            var allManipulators = GetAllManipulators<BaseManipulator>()
                .Concat(GetAllManipulators<ServiceManipulator>())
                .Concat(GetAllManipulators<IndustrialManipulator>());
            return allManipulators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
