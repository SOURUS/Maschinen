namespace Maschinen;

public interface IControlSystem
{
    void setEmptyPlaceSensor(bool value);
    bool getEmptyPlaceSensor();
    void setFullPlaceSensor(bool value);
    bool getFullPlaceSensor();
}