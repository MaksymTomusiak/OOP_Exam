using System.Collections;
using Application.Common.Interfaces;
using Domain;
using Application.Loggers;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Managers
{
    public class ManipulatorManager : IManipulatorManager
    {
        public static ManipulatorManager Instance { get; private set; }
        private readonly IRepository<ServiceManipulator> _serviceRepo;
        private readonly IRepository<IndustrialManipulator> _industrialRepo;
        private readonly IRepository<BaseManipulator> _baseRepo;
        private readonly ILogger _logger;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger>();
            if (Instance != null)
            {
                logger.LogError(null, "ManipulatorManager has already been initialized.");
                return;
            }

            var serviceRepo = serviceProvider.GetRequiredService<IRepository<ServiceManipulator>>();
            var industrialRepo = serviceProvider.GetRequiredService<IRepository<IndustrialManipulator>>();
            var baseRepo = serviceProvider.GetRequiredService<IRepository<BaseManipulator>>();

            Instance = new ManipulatorManager(serviceRepo, industrialRepo, baseRepo, logger);
        }

        private ManipulatorManager(
            IRepository<ServiceManipulator> serviceRepo,
            IRepository<IndustrialManipulator> industrialRepo,
            IRepository<BaseManipulator> baseRepo,
            ILogger logger)
        {
            _serviceRepo = serviceRepo;
            _industrialRepo = industrialRepo;
            _baseRepo = baseRepo;
            _logger = logger;
        }

        public bool AddManipulator(BaseManipulator manipulator)
        {
            try
            {
                switch (manipulator)
                {
                    case ServiceManipulator serviceManipulator:
                        _serviceRepo.Add(serviceManipulator);
                        _logger.LogInfo($"Added ServiceManipulator with id {serviceManipulator.Id} and name {serviceManipulator.Name} to the database.");
                        break;

                    case IndustrialManipulator industrialManipulator:
                        _industrialRepo.Add(industrialManipulator);
                        _logger.LogInfo($"Added IndustrialManipulator with id {industrialManipulator.Id} and name {industrialManipulator.Name} to the database.");
                        break;

                    case BaseManipulator baseManipulator:
                        _baseRepo.Add(baseManipulator);
                        _logger.LogInfo($"Added BaseManipulator with id {baseManipulator.Id} and name {baseManipulator.Name} to the database.");
                        break;

                    default:
                        _logger.LogError(null, $"Unrecognized manipulator type {manipulator.GetType().Name}.");
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding manipulator to the database.");
                return false;
            }
        }

        public bool RemoveManipulator<T>(Guid manipulatorId) where T : BaseManipulator
        {
            try
            {
                IRepository<T> repository = GetRepository<T>();
                var manipulator = repository.Get(manipulatorId);
                repository.Remove(manipulator);
                _logger.LogInfo($"Removed manipulator of type {manipulator.GetType().Name} with id {manipulator.Id} and name {manipulator.Name} from the database.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing manipulator from the database.");
                return false;
            }
        }

        private IRepository<T> GetRepository<T>() where T : BaseManipulator
        {
            return typeof(T) switch
            {
                var type when type == typeof(ServiceManipulator) => (IRepository<T>)_serviceRepo,
                var type when type == typeof(IndustrialManipulator) => (IRepository<T>)_industrialRepo,
                var type when type == typeof(BaseManipulator) => (IRepository<T>)_baseRepo,
                _ => throw new InvalidOperationException("Repository not found for the specified manipulator type."),
            };
        }

        public IEnumerable<T> GetAllManipulators<T>() where T : BaseManipulator
        {
            try
            {
                IRepository<T> repository = GetRepository<T>();
                return repository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manipulators from the database.");
                return Enumerable.Empty<T>();
            }
        }

        public T? GetManipulator<T>(Guid manipulatorId) where T : BaseManipulator
        {
            try
            {
                IRepository<T> repository = GetRepository<T>();
                return repository.Get(manipulatorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting manipulator from the database by id {manipulatorId}.");
                return null;
            }
        }

        public bool UpdateManipulatorPosition(Guid manipulatorId, string newPosition)
        {
            try
            {
                var manipulator = _baseRepo.Get(manipulatorId);
                manipulator.UpdatePosition(newPosition);
                _baseRepo.Update(manipulator);
                _logger.LogInfo($"Updated position of manipulator under id {manipulatorId} to {newPosition} in the database.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating position of manipulator under id {manipulatorId} to {newPosition} in the database.");
                return false;
            }
        }

        public bool Weld(IndustrialManipulator manipulator)
        {
            try
            {
                if (manipulator is null)
                {
                    _logger.LogError(null, $"Null has been passed instead of manipulator");
                    return false;
                }

                manipulator.Weld();
                _industrialRepo.Update(manipulator);
                _logger.LogInfo($"Manipulator under id {manipulator.Id} performed welding.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Manipulator under id {manipulator.Id} couldn't perform welding.");
                return false;
            }
        }

        public bool Serve(ServiceManipulator manipulator)
        {
            try
            {
                if (manipulator is null)
                {
                    _logger.LogError(null, $"Null has been passed instead of manipulator");
                    return false;
                }

                manipulator.Serve();
                _serviceRepo.Update(manipulator);
                _logger.LogInfo($"Manipulator under id {manipulator.Id} performed serving.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Manipulator under id {manipulator.Id} couldn't perform serving.");
                return false;
            }
        }

        public IEnumerator<BaseManipulator> GetEnumerator()
        {
            var allManipulators = GetAllManipulators<BaseManipulator>();
            return allManipulators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
