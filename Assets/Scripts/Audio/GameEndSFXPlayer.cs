using System;
using Gameplay;
using UnityEngine;

namespace Audio
{
    public class GameEndSFXPlayer: MonoBehaviour
    {
        [SerializeField] private float _volume = 1.0f;
        [SerializeField] private AudioClip _winAudio;
        [SerializeField] private AudioClip _loseAudio;

        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            GameManager.Instance.OnGameEnd.AddListener(PlayGameEndAudio);
        }

        private void PlayGameEndAudio(bool isWin)
        {
            var audio = isWin ? _winAudio : _loseAudio;
            
            if (audio is null) return;
            float[] data = new float[audio.samples * audio.channels];
            audio.GetData(data, 0);
            float[] newData = new float[audio.samples * audio.channels];

            for (int i = 0; i < audio.samples; i++)
            {
                newData[i] = data[i] * _volume;
            }

            audio.SetData(newData, 0);
            AudioManager.Instance.PlaySFX(audio, _audioSource);
        }
    }
}