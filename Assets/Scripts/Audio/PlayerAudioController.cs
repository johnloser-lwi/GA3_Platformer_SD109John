using System;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip _jump;
        [SerializeField] private AudioClip _land;
        [SerializeField] private AudioClip _footStep;
        [SerializeField] private AudioClip _takeDamage;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayJump()
        {
            AudioManager.Instance.PlaySFX(_jump, _audioSource);
        }

        public void PlayLand()
        {
            AudioManager.Instance.PlaySFX(_land, _audioSource);
        }

        public void PlayFootstep()
        {
            AudioManager.Instance.PlaySFX(_footStep, _audioSource);
        }

        public void PlayTakeDamage()
        {
            AudioManager.Instance.PlaySFX(_takeDamage, _audioSource);
        }
    }
}