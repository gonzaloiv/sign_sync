using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class SpectrumVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject[] bands;
        [SerializeField] private float heightMultiplier = 10.0f;
        [SerializeField] private FFTWindow fftWindow = FFTWindow.Hamming;

        private int sampleSize = 512;
        private AudioSource audioSource;
        private float[] spectrumData;

        public AudioSource AudioSource { set { audioSource = value; } }

        private void Start()
        {
            spectrumData = new float[sampleSize];
        }

        private void Update()
        {
            if (audioSource == null)
            {
                HideAll();
            }
            else
            {
                audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
                int spectrumSegmentSize = sampleSize / bands.Length;

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

                    Vector3 newScale = bands[i].transform.localScale;
                    newScale.y = average * heightMultiplier;
                    bands[i].transform.localScale = newScale;
                    bands[i].SetActive(true);
                }
            }
        }

        private void HideAll()
        {
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i].SetActive(false);
            }
        }
    }
}