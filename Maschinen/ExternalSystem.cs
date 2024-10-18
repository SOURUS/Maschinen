namespace Maschinen;

public class ExternalControlSystem : IObserver
    {
        private Random _random = new Random();
        private IControlSystem _controller;
        private bool _isEmptyContainerOrdered = false;
        private bool _isFullContainerPickupRequested = false;

        public ExternalControlSystem(SimulationController controller)
        {
            _controller = controller;
            controller.Attach(this);
        }

        public void Start()
        {
            Task.Run(CheckContainerStatus);
        }

        private async Task CheckContainerStatus()
        {
            while (true)
            {
                await CheckEmptyContainer();
                await CheckFullContainer();
                await Task.Delay(1000); // Check every second
            }
        }

        private async Task CheckEmptyContainer()
        {
            if (!_controller.getEmptyPlaceSensor() && !_isEmptyContainerOrdered)
            {
                _isEmptyContainerOrdered = true;
                await DeliverEmptyContainer();
            }
        }

        private async Task CheckFullContainer()
        {
            if (_controller.getFullPlaceSensor() && !_isFullContainerPickupRequested)
            {
                _isFullContainerPickupRequested = true;
                await PickUpFullContainer();
            }
        }

        public void Update(string eventType)
        {
            switch (eventType)
            {
                case "EmptyPlaceAvailable":
                    _isEmptyContainerOrdered = false;
                    break;
                case "FullPlaceOccupied":
                    _isFullContainerPickupRequested = false;
                    break;
            }
        }

        private async Task DeliverEmptyContainer()
        {
            await Task.Delay(_random.Next(1000, 3000)); // Simulate time for delivery
            Console.WriteLine("External system: Empty container was ordered");

            await Task.Delay(2000); // Simulate delivery time
            WriteLineColored("External system: Empty container was delivered", ConsoleColor.Blue);

            _controller.setEmptyPlaceSensor(true);
        }

        private async Task PickUpFullContainer()
        {
            await Task.Delay(_random.Next(1000, 3000)); // Simulate time for pick up
            Console.WriteLine("External system: Finished goods pick up has been requested");

            await Task.Delay(2000); // Simulate pick up time
            WriteLineColored("External system: Finished goods were picked up", ConsoleColor.Green);

            _controller.setFullPlaceSensor(false);
        }

        private static void WriteLineColored(string message, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }