using System;
using UnityEngine;

namespace Audio
{
    public class SimpleAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private bool _playOnStart;
        
        
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            
            if (_playOnStart) PlayAudio();
        }

        public void PlayAudio()
        {
            if (!_audioSource)
            {
                AudioManager.Instance.PlaySFX(_audioClip);
                return;
            }
            AudioManager.Instance.PlaySFX(_audioClip, _audioSource);
        }
    }
}