using System.Collections;
using Health;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    // Since enemies and projectile have similar behaviour when hitting the player, 
    // Put the logic here so they can share this behaviour
    [RequireComponent(typeof(IWeaponizable))]
    public class AttackController : MonoBehaviour
    {
        public UnityEvent OnDestroySelf;
        
        [SerializeField] private Vector2 _pushForce;
        [SerializeField] private bool _destroySelf;
        [SerializeField] private uint _damage = 1;

        [SerializeField] private float _destroyDelay = 0.0f;
        // Since projectile code is not inherited from BaseCharacterController, 
        // use IWeaponizable to define common logic between projectile and characters
        // in this case they all need a GetCharacterController function to notify playerHealth
        // who's dealing damage
        private IWeaponizable _controller;

        private void Start()
        {
            _controller = GetComponent<IWeaponizable>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            DealDamageToPlayer(other);

            if (_destroySelf)
            {
                StartCoroutine(DestroySelfRoutine());
            }
        }

        IEnumerator DestroySelfRoutine()
        {
            OnDestroySelf.Invoke();
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }

        private void DealDamageToPlayer(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            other.gameObject.TryGetComponent(out CharacterHealth playerHealth);
            if (!playerHealth) return;
            playerHealth.TakeDamage(_controller.GetCharacterController(), _damage);
            var rg = other.gameObject.GetComponent<Rigidbody2D>();
            var dir = other.transform.position.x - transform.position.x;
            rg.AddForce(new Vector2((dir > 0 ? 1 : -1) * _pushForce.x, _pushForce.y), ForceMode2D.Impulse);
        }
    }
}