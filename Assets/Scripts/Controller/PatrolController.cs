using System;
using Health;
using UnityEngine;

namespace Controller
{
    public class PatrolController : BaseController
    {
        [SerializeField] private bool _enableGizmos = true;
        
        [SerializeField] private float _patrolDistance = 10.0f;

        [SerializeField] private float _pushigForce = 2.0f;


        private Vector2 _leftEdge;
        private Vector2 _rightEdge;
        private bool _movingRight = true;
        private Vector2 _initialPosition;
        
        protected override void Start()
        {
            base.Start();
            _initialPosition = transform.position;
            
            _leftEdge = new Vector2(_initialPosition.x - _patrolDistance / 2.0f, _initialPosition.y);
            _rightEdge = new Vector2(_initialPosition.x + _patrolDistance / 2.0f, _initialPosition.y);
        }

        protected override void Update()
        {
            base.Update();
            InputHandler();
        }

        private void InputHandler()
        {
            var position = transform.position;
            if (!IsFlipped && position.x > _rightEdge.x || IsFlipped && position.x < _leftEdge.x)
            {
                _movingRight = !_movingRight;
            }
            
            _horizontalAxis = _movingRight ? -1 : 1;
            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            other.gameObject.TryGetComponent<CharacterHealth>(out CharacterHealth playerHealth);
            if (!playerHealth) return;
            playerHealth.TakeDamage(this);
            var rg = other.rigidbody;
            var dir = other.transform.position.x - transform.position.x;
            rg.AddForce(new Vector2(dir > 0 ? 1:-1, 1.0f) * _pushigForce, ForceMode2D.Impulse);
        }

        private void OnDrawGizmos()
        {
            if (!_enableGizmos) return;
            var leftEdge = new Vector2(_initialPosition.x - _patrolDistance / 2.0f, _initialPosition.y);
            var rightEdge = new Vector2(_initialPosition.x + _patrolDistance / 2.0f, _initialPosition.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftEdge, 0.5f);
            Gizmos.DrawWireSphere(rightEdge, 0.5f);
        }
    }
}