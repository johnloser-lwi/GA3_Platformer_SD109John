using System;
using Health;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class ProjectileController : MonoBehaviour, ICharacterComponent
    {
        
        
        [SerializeField] private float _speed = 4.0f;
        [SerializeField] private float _lifeTime = 5.0f;

        private SpriteRenderer _spriteRenderer;
        private BaseCharacterController _owner;
        private bool _isFlipped;
        
        private float _timer;

        public void SetOwner(BaseCharacterController owner)
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

        public BaseCharacterController GetCharacterController()
        {
            // in this case, this Controller is not inherited from BaseCharacterController
            // so we return owner of the projectile which is define when projectile is spawned
            return _owner;
        }
    }
}