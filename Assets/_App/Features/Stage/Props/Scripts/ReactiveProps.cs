using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class ReactiveProps : MonoBehaviour
    {
        private static int SampleSize = 128;

        [SerializeField] private Band[] bands = new Band[7];
        [SerializeField] private FFTWindow fftWindow = FFTWindow.Hamming;

        private float[] spectrumData = new float[SampleSize];
        private AudioSource audioSource;

        public void Show(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            gameObject.SetActive(true);
            foreach (IBandListener listener in GetComponentsInChildren<IBandListener>(true))
            {
                bands[listener.BandIndex].AddListener(listener);
            }
        }

        private void Update()
        {
            if (audioSource != null)
            {
                audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
                int spectrumSegmentSize = SampleSize / bands.Length;
                for (int i = 0; i < bands.Length; i++)
                {
                    float average = 0.0f;
                    int start = i * spectrumSegmentSize;
                    int end = start + spectrumSegmentSize;
                    for (int j = start; j < end; j++)
                    {
                        average += spectrumData[j];
                    }
                    average /= spectrumSegmentSize;
                    bands[i].Update(average);
                }
            }
        }

        public void Hide()
        {
            audioSource = null;
            foreach (Band band in bands)
            {
                band.listeners.Clear();
            }
            gameObject.SetActive(false);
        }
    }
}
