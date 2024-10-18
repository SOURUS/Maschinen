namespace Maschinen
{
   public class SimulationController : IControlSystem, ISubject
    {
        private bool _isEmptyPlaceOccupied = false;
        private bool _isFullPlaceOccupied = false;
        
        private List<Machine> _machines = new List<Machine>();
        private List<IObserver> _observers = new List<IObserver>();
        private const int BaseFillTime = 3000; // 3 seconds for filling up a product
        private readonly object _emptyPlaceLock = new object();
        private readonly object _fullPlaceLock = new object();
        
        public SimulationController()
        {
            for (var i = 0; i < 3; i++)
            {
                var timeOffset = i * (BaseFillTime / 3);
                _machines.Add(new Machine(i + 1, this, BaseFillTime, timeOffset));
            }
        }

        public void Start()
        {
            var externalSystem = new ExternalControlSystem(this);
            foreach (var machine in _machines)
            {
                Task.Run(async () => await machine.RunAsync());
            }
            externalSystem.Start();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Notify(string eventType)
        {
            foreach (var observer in _observers)
            {
                observer.Update(eventType);
            }
        }

        public void setEmptyPlaceSensor(bool value)
        {
            lock (_emptyPlaceLock)
            {
                if (_isEmptyPlaceOccupied != value)
                {
                    _isEmptyPlaceOccupied = value;
                    Notify(value ? "EmptyPlaceOccupied" : "EmptyPlaceAvailable");
                }
            }
        }

        public bool getEmptyPlaceSensor()
        {
            lock (_emptyPlaceLock)
            {
                return _isEmptyPlaceOccupied;
            }
        }

        public void setFullPlaceSensor(bool value)
        {
            lock (_fullPlaceLock)
            {
                if (_isFullPlaceOccupied != value)
                {
                    _isFullPlaceOccupied = value;
                    Notify(value ? "FullPlaceOccupied" : "FullPlaceAvailable");
                }
            }
        }

        public bool getFullPlaceSensor()
        {
            lock (_fullPlaceLock)
            {
                return _isFullPlaceOccupied;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the production station simulator");
            var controller = new SimulationController();
            controller.Start();
            Console.ReadKey();
        }
    }
}