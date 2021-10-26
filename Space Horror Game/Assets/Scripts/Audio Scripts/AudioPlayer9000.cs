using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer9000 : MonoBehaviour
    {
        AudioSource aS;
        Sound.SoundType type;

        private void Start()
        {
            aS = GetComponent<AudioSource>();
            type = AudioManager.Instance.GetSoundTypeFromClip(aS.clip);
            UpdateVolume();
        }
        public void UpdateVolume() => aS.volume = AudioManager.Instance.GetVolumeFromClip(aS.clip) * AudioManager.Instance.GetVolumeFromSoundType(type);
    }
}