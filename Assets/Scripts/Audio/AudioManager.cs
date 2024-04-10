using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _UIAudioSource;
        [SerializeField] private AudioSource _MusicAudioSource;
        [SerializeField] private AudioSource _OtherAudioSource;
        [SerializeField] private float _audioCooldown = 0.2f;
        
        private Dictionary<string, float> _cacheList;

        private void Start()
        {
            _cacheList = new Dictionary<string, float>();
        }

        private void Update()
        {
            if (_cacheList.Count == 0) return;
            string[] keys = new string[_cacheList.Count];
            _cacheList.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                _cacheList[key] -= Time.deltaTime;
                if (_cacheList[key] <= 0) _cacheList.Remove(key);
            }
        }

        public void PlaySFX(AudioClip audio, AudioSource source, bool applyCooldown = false)
        {
            if (!audio || _cacheList.ContainsKey(audio.name)) return;
            source.PlayOneShot(audio);
            if (!applyCooldown) return;
            _cacheList.Add(audio.name, _audioCooldown);
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
            StopSource(_MusicAudioSource);
        }

        public void StopOther()
        {
            StopSource(_OtherAudioSource);
        }

        public void StopUI()
        {
            StopSource(_UIAudioSource);
        }

        private void StopSource(AudioSource source)
        {
            if (source is null) return;
            if (source.isPlaying) source.Stop();
        }

        public void PlayUI(AudioClip audio)
        {
            if (!audio) return;
            PlaySFX(audio, _UIAudioSource);
        }
        
        public void PlaySFX(AudioClip audio)
        {
            if (!audio) return;
            PlaySFX(audio, _OtherAudioSource);
        }

        public void PlayRandomSFX(RandomAudioClip clip, AudioSource source, bool applyCooldown = false)
        {
            var audio = clip.PickRandom();
            if (audio == null) return;
            PlaySFX(audio, source, applyCooldown);
        }
    }
}