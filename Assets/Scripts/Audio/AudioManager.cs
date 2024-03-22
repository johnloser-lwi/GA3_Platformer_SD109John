using UnityEngine;
using Utility;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _UIAudioSource;
        [SerializeField] private AudioSource _MusicAudioSource;

        public void PlaySFX(AudioClip audio, AudioSource source)
        {
            if (!audio) return;
            source.PlayOneShot(audio);
        }

        public void PlayMusic(AudioClip audio)
        {
            if (!audio) return;
            if (_MusicAudioSource.isPlaying) _MusicAudioSource.Stop();
            
            _MusicAudioSource.clip = audio;
            _MusicAudioSource.loop = true;
            _MusicAudioSource.Play();
        }

        public void StopMusic()
        {
            if (_MusicAudioSource.isPlaying) _MusicAudioSource.Stop();
        }

        public void PlayUI(AudioClip audio)
        {
            if (!audio) return;
            PlaySFX(audio, _UIAudioSource);
        }
    }
}