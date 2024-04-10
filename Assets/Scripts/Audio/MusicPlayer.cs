using System;
using UnityEngine;

namespace Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _music;

        private void Start()
        {
            if (_music is null) return; 
            AudioManager.Instance.PlayMusic(_music);
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopMusic();
        }
    }
}