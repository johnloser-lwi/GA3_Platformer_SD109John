using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private float _moveRange = 10.0f;
        [SerializeField] private float _moveSpeed = 2.0f;
        
        private Vector2 _initialPosition;
        private Vector2 _leftPosition;
        private Vector2 _rightPosition;
        private bool _movingRight = true;
        
        
        private void Start()
        {
            _initialPosition = transform.position;
            _leftPosition = _initialPosition + Vector2.left * _moveRange;
            _rightPosition = _initialPosition + Vector2.right * _moveRange;
            
#if UNITY_EDITOR
            var col = GetComponentInChildren<TilemapCollider2D>();
            if (!col) return;
            _initialPositionGizmos = col.bounds.center;
            _leftPositionGizmos = _initialPositionGizmos + Vector3.left * _moveRange;
            _rightPositionGizmos = _initialPositionGizmos + Vector3.right * _moveRange;
#endif
        }
        
        private void FixedUpdate()
        {
            var position = transform.position;
            if (position.x >= _rightPosition.x)
            {
                _movingRight = false;
            }
            else if (position.x <= _leftPosition.x)
            {
                _movingRight = true;
            }
            
            
            if (_movingRight)
            {
                position = Vector2.MoveTowards(position, _rightPosition, _moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                position = Vector2.MoveTowards(position, _leftPosition, _moveSpeed * Time.fixedDeltaTime);
            }
            
            transform.position = position;
        }
        
#if UNITY_EDITOR
        private Vector3 _initialPositionGizmos;
        private Vector3 _leftPositionGizmos;
        private Vector3 _rightPositionGizmos;
        private void OnDrawGizmos()
        {
            var tileCollider = GetComponentInChildren<TilemapCollider2D>();
            if (tileCollider == null) return;
            var isGameplay = Application.isPlaying;
            var position = isGameplay ? _initialPositionGizmos : tileCollider.bounds.center;
            var leftPosition = isGameplay ? _leftPositionGizmos : position + Vector3.left * _moveRange;
            var rightPosition = isGameplay ? _rightPositionGizmos : position + Vector3.right * _moveRange;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(leftPosition, tileCollider.bounds.size);
            Gizmos.DrawWireCube(rightPosition, tileCollider.bounds.size);
        }
#endif
    }
}