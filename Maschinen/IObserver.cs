namespace Maschinen;

public interface IObserver
{
    void Update(string eventType);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Notify(string eventType);
}