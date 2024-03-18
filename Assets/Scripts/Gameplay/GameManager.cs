using UnityEngine.SceneManagement;
using UnityEngine;
using Utility;

namespace Gameplay
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(0);
        }

        private void Update()
        {
            InputUpdate();
        }

        private void InputUpdate()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                ResetLevel();
            }
        }
    }
}

