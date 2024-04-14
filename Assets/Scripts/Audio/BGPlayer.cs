
using UnityEngine;

namespace Audio
{
    public class BGPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _bg;

        private void Start()
        {
            if (_bg is null) return;
            AudioManager.Instance.PlayBG(_bg);
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopBG();
        }
    }
}