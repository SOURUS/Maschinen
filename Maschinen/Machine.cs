namespace Maschinen;

public class Machine
{
    private readonly int _id;
    private readonly IControlSystem _controlSystem;
    private readonly int _baseFillTime;
    private readonly Random _random = new Random();
    private readonly int _timeOffset;

    public Machine(int id, IControlSystem controlSystem, int baseFillTime, int timeOffset)
    {
        _id = id;
        _controlSystem = controlSystem;
        _baseFillTime = baseFillTime;
        _timeOffset = timeOffset;
    }

    public async Task RunAsync()
    {
        await Task.Delay(_timeOffset);

        while (true)
        {
            await WaitForEmptyPlace();
            await ProcessItem();
            await WaitForFullPlace();
            await PlaceFullItem();

            var deviation = _random.Next(-500, 500);
            await Task.Delay(_baseFillTime + deviation);
        }
    }

    private async Task WaitForEmptyPlace()
    {
        while (!_controlSystem.getEmptyPlaceSensor())
        {
            await Task.Delay(300);
        }

        _controlSystem.setEmptyPlaceSensor(false);
        Console.WriteLine($"Machine #{_id}: Start Working");
    }

    private async Task ProcessItem()
    {
        var processingTime = _baseFillTime + _random.Next(-200, 200);
        await Task.Delay(processingTime);
        Console.WriteLine($"Machine #{_id}: Process Finished");
    }

    private async Task WaitForFullPlace()
    {
        while (_controlSystem.getFullPlaceSensor())
        {
            await Task.Delay(300);
        }
    }

    private async Task PlaceFullItem()
    {
        await Task.Delay(300);
        _controlSystem.setFullPlaceSensor(true);
        Console.WriteLine($"Machine #{_id}: The product is released");
    }
}