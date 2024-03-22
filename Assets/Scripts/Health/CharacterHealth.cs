
using System;
using Controller;
using Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BaseCharacterController))]
    public class CharacterHealth : MonoBehaviour
    {
        // Properties
        public uint Health
        {
            get => _health;
            private set
            {
                if (value != _health)
                {
                    _health = value;
                    OnHealthChange.Invoke(value);
                }
            }
        }
        public bool IsDead => Health == 0;
        
        // Serialize fields
        [Header("Settings")]
        [SerializeField] private uint _health = 3;
        [SerializeField] private float _fallDamageThreshold = -10.0f;

        // Events
        [Space(10)]
        [Header("Events")]
        public UnityEvent OnDead;
        public UnityEvent<BaseCharacterController> OnTakeDamage;
        public UnityEvent<uint> OnHealthChange;

        // Private fields
        private Rigidbody2D _rigidbody;
        private BaseCharacterController _characterController;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _characterController = GetComponent<BaseCharacterController>();

            Health = _health;
            
            OnHealthChange.Invoke(Health);
        }

        public void TakeDamage(BaseCharacterController source)
        {
            if (IsDead) return;
            Health--;
            if (_health > 0)
            {
                OnTakeDamage.Invoke(source);
#if DEBUG
                var msg = source == _characterController ? 
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
            
            TakeDamage(_characterController);
        }
    }
}