using UnityEngine;

namespace Audio
{
    [System.Serializable] public class Sound
    {
        public enum SoundType { Ambiance, UI, Effects, Voice }
        public SoundType soundType;

        public AudioClip audioClip;

        [Range(0, 1)] public float Volume;
        public bool loop;
    }
}