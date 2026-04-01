namespace DigitalLove.Game.Stage
{
    public interface IBandListener
    {
        int BandIndex { get; }
        void OnUpdate(float value);
    }
}
