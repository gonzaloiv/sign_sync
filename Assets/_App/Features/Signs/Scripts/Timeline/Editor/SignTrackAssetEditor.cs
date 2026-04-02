using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace DigitalLove.Game.Signs
{
    [CustomTimelineEditor(typeof(SignTrackAsset))]
    public class SignTrackAssetEditor : ClipEditor
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            ClipDrawOptions clipOptions = base.GetClipOptions(clip);
            clipOptions.highlightColor = GetColor(clip);
            return clipOptions;
        }

        private Color GetColor(TimelineClip clip)
        {
            switch (((SignTrackAsset)clip.asset).signId)
            {
                case SignId.Fist:
                    return Color.red;
                case SignId.Paper:
                    return Color.blue;
                case SignId.Scissors:
                    return Color.limeGreen;
                case SignId.Stop:
                    return Color.yellow;
                default:
                    return Color.gray;
            }
        }
    }
}