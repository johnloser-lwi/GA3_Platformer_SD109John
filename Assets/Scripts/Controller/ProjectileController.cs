using System;
using Health;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class ProjectileController : MonoBehaviour, ICharacterController
    {
        
        
        [SerializeField] private float _speed = 4.0f;
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
            if (_timer > 0) return;
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            // move
            var dir = _spriteRenderer.flipX ? -1 : 1;
            
            transform.Translate(_speed * Time.deltaTime * dir, 0, 0);
        }

        public BaseController GetController()
        {
            return _owner;
        }
    }
}