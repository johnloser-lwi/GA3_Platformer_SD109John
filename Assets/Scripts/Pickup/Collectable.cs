using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup
{
    public class Collectable : MonoBehaviour
    {
        public UnityEvent OnCollect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            OnCollect.Invoke();
            Destroy(gameObject);
        }
    }
}