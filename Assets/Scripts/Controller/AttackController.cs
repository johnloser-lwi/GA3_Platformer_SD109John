using Health;
using UnityEngine;

namespace Controller
{
    // Since enemies and projectile have similar behaviour when hitting the player, 
    // Put the logic here so they can share this behaviour
    [RequireComponent(typeof(ICharacterComponent))]
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private Vector2 _pushForce;
        [SerializeField] private bool _destroySelf;

        // Since projectile code is not inherited from BaseCharacterController, 
        // use ICharacterComponent to define common logic between projectile and characters
        // in this case they all need a GetCharacterController function to notify playerHealth
        // who's dealing damage
        private ICharacterComponent _controller;

        private void Start()
        {
            _controller = GetComponent<ICharacterComponent>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.TryGetComponent(out CharacterHealth playerHealth);
                if (!playerHealth) return;
                playerHealth.TakeDamage(_controller.GetCharacterController());
                var rg = other.gameObject.GetComponent<Rigidbody2D>();
                var dir = other.transform.position.x - transform.position.x;
                rg.AddForce(new Vector2((dir > 0 ? 1 : -1) * _pushForce.x, _pushForce.y), ForceMode2D.Impulse);
            }
            
            if (_destroySelf) Destroy(gameObject);
        }
    }
}