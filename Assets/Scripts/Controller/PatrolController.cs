﻿using Health;
using UnityEngine;

namespace Controller
{
    public class PatrolController : BaseController
    {
        [Space(10)]
        [Header("Patrol Settings")]
#if UNITY_EDITOR
        [SerializeField] private bool _enableGizmos = true;
#endif
        [SerializeField] private float _patrolDistance = 10.0f;


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


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_enableGizmos) return;
            var position = transform.position;
            if (Application.isPlaying) position = _initialPosition;
            var leftEdge = new Vector2(position.x - _patrolDistance / 2.0f, position.y);
            var rightEdge = new Vector2(position.x + _patrolDistance / 2.0f, position.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftEdge, 0.5f);
            Gizmos.DrawWireSphere(rightEdge, 0.5f);
        }
#endif

    }
}