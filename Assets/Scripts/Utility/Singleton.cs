using UnityEngine;

namespace Utility
{
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance is null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    Instantiate(go);
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = gameObject.GetComponent<T>();
        }
    }
}