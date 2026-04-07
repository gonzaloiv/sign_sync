using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DigitalLove.Game.Signs
{
    public class SignTrackMixerBehaviour : PlayableBehaviour
    {
        private BaseRecogniser recogniser;
        private List<SignTrackBehaviour> behaviours = new();

        public override void OnGraphStart(Playable playable)
        {
            behaviours.Clear();
        }

        public override void OnGraphStop(Playable playable)
        {
            behaviours.Clear();
        }

        public override void ProcessFrame(Playable mixer, FrameData info, object playerData)
        {
            if (recogniser == null)
                recogniser = playerData as BaseRecogniser;
            if (recogniser == null)
                return;

            bool isActuallyPlaying = info.evaluationType == FrameData.EvaluationType.Playback;
            int inputCount = mixer.GetInputCount();
            double timelineTime = mixer.GetGraph().GetRootPlayable(0).GetTime();
            for (int i = 0; i < inputCount; i++)
            {
                SignTrackBehaviour behaviour = ((ScriptPlayable<SignTrackBehaviour>)mixer.GetInput(i)).GetBehaviour();
                if (behaviour != null)
                {
                    bool isTime = timelineTime > behaviour.startTime - recogniser.ActiveSecs;
                    if (isTime && !behaviours.Contains(behaviour))
                    {
                        // Debug.LogWarning($"Current Sign Id {behaviour.signId} and Hand Id {spawner.HandId}");
                        if (Application.isPlaying && isActuallyPlaying)
                        {

                            float duration = (float)(behaviour.finalTime - behaviour.startTime);
                            recogniser.ListenTo(behaviour.signId, duration);
                        }
                        behaviours.Add(behaviour);
                    }
                }
            }
        }
    }
}