using System;
using Health;
using UnityEngine;

namespace Controller
{
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private Vector2 _pushForce;
        [SerializeField] private bool _destroySelf = false;

        private ICharacterController _controller;

        private void Start()
        {
            _controller = GetComponent<ICharacterController>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.TryGetComponent(out CharacterHealth playerHealth);
                if (!playerHealth) return;
                playerHealth.TakeDamage(_controller.GetController());
                var rg = other.gameObject.GetComponent<Rigidbody2D>();
                var dir = other.transform.position.x - transform.position.x;
                rg.AddForce(new Vector2((dir > 0 ? 1 : -1) * _pushForce.x, _pushForce.y), ForceMode2D.Impulse);
            }
            
            if (_destroySelf) Destroy(gameObject);
        }
    }
}