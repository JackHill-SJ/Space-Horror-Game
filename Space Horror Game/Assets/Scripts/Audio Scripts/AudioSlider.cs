using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour
    {
        public Sound.SoundType type;
        Slider s;

        void Start()
        {
            s = GetComponent<Slider>();
            s.value = AudioManager.Instance.GetVolumeFromSoundType(type);
            s.onValueChanged.AddListener((float x) => SetVolume(x));
        }

        private void SetVolume(float value) => AudioManager.Instance.SetVolumeFromSoundType(value, type);
    }
}