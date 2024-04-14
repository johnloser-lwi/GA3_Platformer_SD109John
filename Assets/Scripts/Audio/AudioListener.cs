using System;
using UnityEngine;

namespace Audio
{
    public class AudioListener : MonoBehaviour
    {
        [SerializeField] private Transform _follow;

        private void FixedUpdate()
        {
            if (_follow is null) return;

            transform.position = _follow.position;
        }
    }
}