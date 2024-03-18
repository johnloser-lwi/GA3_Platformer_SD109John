
using System;
using Controller;
using Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BaseController))]
    public class CharacterHealth : MonoBehaviour
    {
        // Properties
        public uint Health => _health;
        public bool IsDead => _health == 0;
        
        // Serialize fields
        [Header("Settings")]
        [SerializeField] private uint _health = 3;
        [SerializeField] private float _fallDamageThreshold = -10.0f;

        // Events
        [Space(10)]
        [Header("Events")]
        public UnityEvent OnDead;
        public UnityEvent<BaseController> OnTakeDamage;

        // Private fields
        private Rigidbody2D _rigidbody;
        private BaseController _controller;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _controller = GetComponent<BaseController>();

            if (gameObject.CompareTag("Player"))
            {
                OnDead.AddListener(GameManager.Instance.ResetLevel);
            }
        }

        public void TakeDamage(BaseController source)
        {
            if (IsDead) return;
            _health--;
            if (_health > 0)
            {
                OnTakeDamage.Invoke(source);
#if DEBUG
                var msg = source == _controller ? 
                    $"{gameObject.name} is taking fall damage" : $"{gameObject.name} is taking damage from {source.gameObject}";
                Debug.Log(msg);
#endif
                return;
            }
            
            OnDead.Invoke();
#if DEBUG
            Debug.Log($"{gameObject.name} is dead");
#endif
        }

        private void FixedUpdate()
        {
            if (_rigidbody.velocity.y > _fallDamageThreshold) return;
            
            TakeDamage(_controller);
        }
    }
}