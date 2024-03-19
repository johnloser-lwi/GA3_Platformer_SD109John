using System;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TurretController : MonoBehaviour
    {
        [SerializeField] private ProjectileController _projectile;
        [SerializeField] private float _shootingInterval = 2.0f;
        [SerializeField] private GameObject _projectileSpawnPoint;
        
        
        private Transform _playerTransform;
        private SpriteRenderer _spriteRenderer;

        private float _shootTimer;
        
        private void Start()
        {
            var player = GameObject.FindWithTag("Player");
            if (player) _playerTransform = player.transform;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            ResetShootTimer();
        }

        private void ResetShootTimer()
        {
            _shootTimer = _shootingInterval;
        }

        private void Update()
        {
            Flip();
            Shooting();
        }

        private void Shooting()
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer > 0) return;

            if (_projectile)
            {
                if (_spriteRenderer.flipX) _projectile.Flip();
                Instantiate(_projectile, _projectileSpawnPoint.transform.position, Quaternion.identity);
            }
            
            ResetShootTimer();
        }

        private void Flip()
        {
            var playerX = _playerTransform.position.x;
            var x = transform.position.x;

            if (x > playerX)
            {
                _spriteRenderer.flipX = true;
            }
            else if (x < playerX)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
}