using System;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAudioController : MonoBehaviour
    {
        [SerializeField] private RandomAudioClip _jump;
        [SerializeField] private AudioClip _land;
        [SerializeField] private AudioClip _footStep;
        [SerializeField] private RandomAudioClip _takeDamage;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayJump()
        {
            AudioManager.Instance.PlayRandomSFX(_jump, _audioSource);
        }

        public void PlayLand()
        {
            AudioManager.Instance.PlaySFX(_land, _audioSource, true);
        }

        public void PlayFootstep()
        {
            AudioManager.Instance.PlaySFX(_footStep, _audioSource, true);
        }

        public void PlayTakeDamage()
        {
            AudioManager.Instance.PlayRandomSFX(_takeDamage, _audioSource);
        }
    }
}