using System;
using Health;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class ProjectileController : MonoBehaviour
    {
        
        
        [SerializeField] private float _speed = 4.0f;
        [SerializeField] private float _pushigForce = 12.0f;

        private SpriteRenderer _spriteRenderer;
        private BaseController _owner;

        public void SetOwner(BaseController owner)
        {
            _owner = owner;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GetComponent<Collider2D>().isTrigger = true;
        }

        public void Flip()
        {
            _spriteRenderer.flipX = true;
        }

        private void Update()
        {
            // move
            var dir = _spriteRenderer.flipX ? -1 : 1;
            
            transform.Translate(_speed * Time.deltaTime * dir, 0, 0);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                Destroy(gameObject);
            
            other.gameObject.TryGetComponent<CharacterHealth>(out CharacterHealth playerHealth);
            if (!playerHealth) return;
            playerHealth.TakeDamage(_owner);
            var rg = other.rigidbody;
            var dir = other.transform.position.x - transform.position.x;
            rg.AddForce(new Vector2(dir > 0 ? 1:-1, 0) * _pushigForce, ForceMode2D.Impulse);
        }
    }
}