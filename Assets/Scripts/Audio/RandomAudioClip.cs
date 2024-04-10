using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    [Serializable]
    public class RandomAudioClip
    {
        [SerializeField] private AudioClip[] clips;

        [CanBeNull]
        public AudioClip PickRandom()
        {
            if (clips.Length == 0) return null;
            
            var index = Random.Range(0, clips.Length);

            return clips[index];
        }
    }
}