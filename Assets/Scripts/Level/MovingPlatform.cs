using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector2 _moveRange;
        [SerializeField] private float _moveSpeed = 2.0f;
        
        private Vector2 _initialPosition;
        private Vector2 _leftPosition;
        private Vector2 _rightPosition;
        private bool _movingRight = true;
        private Vector2 _currentVel;
        
        
        private void Start()
        {
            _initialPosition = transform.position;
            _leftPosition = _initialPosition + Vector2.left * _moveRange.x - Vector2.up * _moveRange.y;
            _rightPosition = _initialPosition + Vector2.right * _moveRange + Vector2.up * _moveRange.y;
            
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
            if (_movingRight && Vector2.Distance(position, _rightPosition) < 0.5f ||
                !_movingRight && Vector2.Distance(position, _leftPosition) < 0.5f)
            {
                _movingRight = !_movingRight;
            }
            
            
            if (_movingRight)
            {
                position = Vector2.MoveTowards(position, _rightPosition, _moveSpeed * Time.deltaTime);
            }
            else
            {
                position = Vector2.MoveTowards(position, _leftPosition, _moveSpeed * Time.deltaTime);
            }
            
            _currentVel =  position - transform.position;
            transform.position = position;
        }

        public Vector2 GetSpeed()
        {
            return _currentVel / Time.deltaTime; 
        }
        
#if UNITY_EDITOR
        private Vector2 _initialPositionGizmos;
        private Vector2 _leftPositionGizmos;
        private Vector2 _rightPositionGizmos;
        private void OnDrawGizmos()
        {
            var tileCollider = GetComponentInChildren<TilemapCollider2D>();
            if (tileCollider == null) return;
            var isGameplay = Application.isPlaying;
            var bounds = tileCollider.bounds;
            var position = isGameplay ? _initialPositionGizmos : new Vector2(bounds.center.x, bounds.center.y);
            var leftPosition = isGameplay ? _leftPositionGizmos : position + Vector2.left * _moveRange.x - Vector2.up * _moveRange.y;
            var rightPosition = isGameplay ? _rightPositionGizmos : position + Vector2.right * _moveRange.x + Vector2.up * _moveRange.y;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(leftPosition, tileCollider.bounds.size);
            Gizmos.DrawWireCube(rightPosition, tileCollider.bounds.size);
        }
#endif
    }
}