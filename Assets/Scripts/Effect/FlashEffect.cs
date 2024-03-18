using System;
using System.Collections;
using UnityEngine;

namespace Effect
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField] private float _flashTime = 0.2f;
        
        private SpriteRenderer _spriteRenderer;
        private WaitForSeconds _waitForSeconds;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _waitForSeconds = new WaitForSeconds(_flashTime);
        }

        public void Flash()
        {
            StartCoroutine(FlashEnumerator());
        }

        private IEnumerator FlashEnumerator()
        {
            _spriteRenderer.color = Color.red;
            yield return _waitForSeconds;
            _spriteRenderer.color = Color.white;
        }
    }
}

