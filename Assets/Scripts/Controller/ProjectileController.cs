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
        [SerializeField] private float _lifeTime = 5.0f;

        private SpriteRenderer _spriteRenderer;
        private BaseController _owner;
        private bool _isFlipped;
        
        private float _timer;

        public void SetOwner(BaseController owner)
        {
            _owner = owner;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _timer = _lifeTime;
            _spriteRenderer.flipX = _isFlipped;
        }

        public void Flip()
        {
            _isFlipped = true;
        }

        private void Update()
        {
            // life time
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Destroy(gameObject);
                return;
            }
            
            // move
            var dir = _spriteRenderer.flipX ? -1 : 1;
            
            transform.Translate(_speed * Time.deltaTime * dir, 0, 0);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy")) return;
            
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.TryGetComponent(out CharacterHealth playerHealth);
                if (!playerHealth) return;
                playerHealth.TakeDamage(_owner);
                var rg = other.gameObject.GetComponent<Rigidbody2D>();
                
                if (rg)
                {
                    var dir = other.transform.position.x - transform.position.x;
                    rg.AddForce(new Vector2(dir > 0 ? 1:-1, 0.2f) * _pushigForce, ForceMode2D.Impulse);
                }
            }
            
           
            Destroy(gameObject);
        }
    }
}