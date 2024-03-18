using System;
using Health;
using UnityEngine.SceneManagement;
using UnityEngine;
using Utility;

namespace Gameplay
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {
            SetupPlayerEvent();
        }

        private void SetupPlayerEvent()
        {
            var player = GameObject.FindWithTag("Player");
            if (!player) return;
            player.TryGetComponent<CharacterHealth>(out CharacterHealth playerHealth);
            if (!playerHealth) return;
            playerHealth.OnDead.AddListener(ResetLevel);
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

