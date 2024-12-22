using Application.Managers;
using Application.ManipulatorFactories;
using Application.Wrapper;
using Domain;

namespace Application.Invoker
{
    public enum Scenario
    {
        AddManipulator = 0,
        UpdateManipulatorPosition,
        RemoveManipulator,
        DisplayAllManipulators,
        PerformServe,
        PerformWeld,
        Exit
    }

    public class Invoker
    {
        private readonly IManipulatorManager _manager;
        private readonly ServiceManipulatorFactory _serviceManipulatorFactory;
        private readonly IndustrialManipulatorFactory _industrialManipulatorFactory;
        private readonly IConsoleWrapper _consoleWrapper;

        private readonly Dictionary<Scenario, Action> _scenarios;

        public Invoker(
            IManipulatorManager manager,
            ServiceManipulatorFactory serviceManipulatorFactory,
            IndustrialManipulatorFactory industrialManipulatorFactory,
            IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
            _manager = manager;
            _serviceManipulatorFactory = serviceManipulatorFactory;
            _industrialManipulatorFactory = industrialManipulatorFactory;

            _scenarios = new Dictionary<Scenario, Action>
            {
                { Scenario.AddManipulator, AddManipulator },
                { Scenario.UpdateManipulatorPosition, UpdateManipulatorPosition },
                { Scenario.RemoveManipulator, RemoveManipulator },
                { Scenario.DisplayAllManipulators, DisplayAllManipulators },
                { Scenario.PerformServe, PerformServe },
                { Scenario.PerformWeld, PerformWeld },
                { Scenario.Exit, Exit }
            };
        }

        public void Run()
        {
            while (true)
            {
                _consoleWrapper.WriteLine("\nChoose an operation:");
                foreach (var scenario in _scenarios.Keys)
                {
                    _consoleWrapper.WriteLine($"{(int)scenario}- {scenario}");
                }

                _consoleWrapper.WriteLine("Enter your choice: ");
                var choice = _consoleWrapper.ReadLine();

                if (int.TryParse(choice, out var option) && Enum.IsDefined(typeof(Scenario), option))
                {
                    var scenario = (Scenario)option;
                    _scenarios[scenario].Invoke();
                }
                else
                {
                    _consoleWrapper.WriteLine("Invalid choice. Try again.");
                }
            }
        }

        private void AddManipulator()
        {
            _consoleWrapper.WriteLine("Enter manipulator type (Service/Industrial): ");
            var type = _consoleWrapper.ReadLine()?.ToLower();

            _consoleWrapper.WriteLine("Enter manipulator name: ");
            var name = _consoleWrapper.ReadLine();

            _consoleWrapper.WriteLine("Enter initial position: ");
            var position = _consoleWrapper.ReadLine();
            BaseManipulator? manipulator = null;
            try
            {
                manipulator = type switch
                {
                    "service" => _serviceManipulatorFactory.CreateManipulator(name, position),
                    "industrial" => _industrialManipulatorFactory.CreateManipulator(name, position),
                    _ => null
                };
            }
            catch(Exception ex)
            {
                _consoleWrapper.WriteLine($"Could not create manipulator.\nDetails: {ex.Message}");
            }

            if (manipulator == null)
            {
                _consoleWrapper.WriteLine("Invalid manipulator type.");
                return;
            }

            var success = _manager.AddManipulator(manipulator);
            _consoleWrapper.WriteLine(success ? "Manipulator added successfully." : "Failed to add manipulator.");
        }

        private void UpdateManipulatorPosition()
        {
            _consoleWrapper.WriteLine("Enter manipulator ID: ");
            if (!Guid.TryParse(_consoleWrapper.ReadLine(), out var id))
            {
                _consoleWrapper.WriteLine("Invalid ID format.");
                return;
            }

            _consoleWrapper.WriteLine("Enter new position: ");
            var newPosition = _consoleWrapper.ReadLine();

            var success = _manager.UpdateManipulatorPosition(id, newPosition);
            _consoleWrapper.WriteLine(success ? "Position updated successfully." : "Failed to update position.");
        }

        private void RemoveManipulator()
        {
            _consoleWrapper.WriteLine("Enter manipulator type (Service/Industrial): ");
            var type = _consoleWrapper.ReadLine()?.ToLower();

            _consoleWrapper.WriteLine("Enter manipulator ID: ");
            if (!Guid.TryParse(_consoleWrapper.ReadLine(), out var id))
            {
                _consoleWrapper.WriteLine("Invalid ID format.");
                return;
            }

            var success = type switch
            {
                "service" => _manager.RemoveManipulator<ServiceManipulator>(id),
                "industrial" => _manager.RemoveManipulator<IndustrialManipulator>(id),
                _ => false
            };

            _consoleWrapper.WriteLine(success ? "Manipulator removed successfully." : "Failed to remove manipulator.");
        }

        private void DisplayAllManipulators()
        {
            _consoleWrapper.WriteLine("Enter manipulator type (Service/Industrial): ");
            var type = _consoleWrapper.ReadLine()?.ToLower();

            IEnumerable<BaseManipulator>? manipulators = type switch
            {
                "service" => _manager.GetAllManipulators<ServiceManipulator>(),
                "industrial" => _manager.GetAllManipulators<IndustrialManipulator>(),
                _ => null
            };

            if (manipulators == null)
            {
                _consoleWrapper.WriteLine("Invalid manipulator type.");
                return;
            }

            _consoleWrapper.WriteLine("Manipulators:");
            foreach (var manipulator in manipulators)
            {
                _consoleWrapper.WriteLine($"ID: {manipulator.Id}, Name: {manipulator.Name}, Position: {manipulator.Position}");
            }
        }

        private void PerformServe()
        {
            _consoleWrapper.WriteLine("Enter manipulator ID: ");
            if (!Guid.TryParse(_consoleWrapper.ReadLine(), out var id))
            {
                _consoleWrapper.WriteLine("Invalid ID format.");
                return;
            }

            var manipulator = _manager.GetManipulator<ServiceManipulator>(id);
            if (manipulator == null)
            {
                _consoleWrapper.WriteLine("Manipulator not found.");
                return;
            }

            var success = _manager.Serve(manipulator);
            _consoleWrapper.WriteLine(success ? "Manipulator performed serve operation successfully." : "Failed to perform serve operation.");
        }

        private void PerformWeld()
        {
            _consoleWrapper.WriteLine("Enter manipulator ID: ");
            if (!Guid.TryParse(_consoleWrapper.ReadLine(), out var id))
            {
                _consoleWrapper.WriteLine("Invalid ID format.");
                return;
            }

            var manipulator = _manager.GetManipulator<IndustrialManipulator>(id);
            if (manipulator == null)
            {
                _consoleWrapper.WriteLine("Manipulator not found.");
                return;
            }

            var success = _manager.Weld(manipulator);
            _consoleWrapper.WriteLine(success ? "Manipulator performed weld operation successfully." : "Failed to perform weld operation.");
        }

        private void Exit()
        {
            _consoleWrapper.WriteLine("Exiting the application. Goodbye!");
            Environment.Exit(0);
        }
    }
}
