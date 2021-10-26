using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] sounds = new Sound[0];
        [HideInInspector] public List<AudioPlayer9000> audioPlayers = new List<AudioPlayer9000>();

        [Range(0, 1)] public float AmbianceVolume = 1;
        [Range(0, 1)] public float UIVolume = 1;
        [Range(0, 1)] public float EffectsVolume = 1;
        [Range(0, 1)] public float VoiceVolume = 1;

        //save stuff
        private SaveDataGeneric<float> AmbianceVolumeSave = null;
        private SaveDataGeneric<float> UIVolumeSave = null;
        private SaveDataGeneric<float> EffectsVolumeSave = null;
        private SaveDataGeneric<float> VoiceVolumeSave = null;

        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSaveData();
        }

        private void InitSaveData()
        {
            AmbianceVolumeSave = new SaveDataGeneric<float>(nameof(AmbianceVolumeSave), 1);
            UIVolumeSave = new SaveDataGeneric<float>(nameof(UIVolumeSave), 1);
            EffectsVolumeSave = new SaveDataGeneric<float>(nameof(EffectsVolumeSave), 1);
            VoiceVolumeSave = new SaveDataGeneric<float>(nameof(VoiceVolumeSave), 1);

            AmbianceVolume = AmbianceVolumeSave.Value;
            UIVolume = UIVolumeSave.Value;
            EffectsVolume = EffectsVolumeSave.Value;
            VoiceVolume = VoiceVolumeSave.Value;
        }
        private void OverrideSaveData()
        {
            AmbianceVolumeSave.Value = AmbianceVolume;
            UIVolumeSave.Value = UIVolume;
            EffectsVolumeSave.Value = EffectsVolume;
            VoiceVolumeSave.Value = VoiceVolume;

            foreach (AudioPlayer9000 aP in audioPlayers) aP.UpdateVolume();
        }

        public Sound.SoundType GetSoundTypeFromClip(AudioClip audioClip) => System.Array.Find(sounds, sound => sound.audioClip == audioClip).soundType;
        public float GetVolumeFromClip(AudioClip audioClip) => System.Array.Find(sounds, sound => sound.audioClip == audioClip).Volume;
        public float GetVolumeFromSoundType(Sound.SoundType soundType) => soundType switch { Sound.SoundType.Ambiance => AmbianceVolume, Sound.SoundType.UI => UIVolume, Sound.SoundType.Effects => EffectsVolume, Sound.SoundType.Voice => VoiceVolume, _ => 0, };
        public void SetVolumeFromSoundType(float volume, Sound.SoundType soundType)
        {
            switch (soundType)
            {
                case Sound.SoundType.Ambiance:
                    AmbianceVolume = volume;
                    break;
                case Sound.SoundType.UI:
                    UIVolume = volume;
                    break;
                case Sound.SoundType.Effects:
                    EffectsVolume = volume;
                    break;
                case Sound.SoundType.Voice:
                    VoiceVolume = volume;
                    break;
            }
            OverrideSaveData();
        }
    }
}