using UnityEngine;

namespace Controller
{
    public class TurretController : BaseController
    {
        [Space(10)]
        [Header("Turret Settings")]
        [SerializeField] private ProjectileController _projectile;
        [SerializeField] private float _shootingInterval = 2.0f;
        [SerializeField] private GameObject _projectileSpawnPoint;
        
        
        private Transform _playerTransform;
        private float _shootTimer;
        
        protected override void Start()
        {
            base.Start();
            var player = GameObject.FindWithTag("Player");
            if (player) _playerTransform = player.transform;

            ResetShootTimer();
        }

        private void ResetShootTimer()
        {
            _shootTimer = _shootingInterval;
        }

        protected override void Update()
        {
            base.Update();
            Flip();
            Shooting();
        }

        private void Shooting()
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer > 0) return;

            if (_projectile)
            {
               
                var go = Instantiate(_projectile, _projectileSpawnPoint.transform.position, Quaternion.identity);
                if (_spriteRenderer.flipX) go.Flip();
                go.SetOwner(this);
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