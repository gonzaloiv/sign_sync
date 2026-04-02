namespace DigitalLove.Game.Signs
{
    public class HandSignListener
    {
        public SignId signId;
        public float launchTime;
        public float duration;
        public SignVisual visual;
        public int frames;
        public float percentage;

        public bool HasBeenRecognised => frames > 0;
    }
}