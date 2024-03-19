using Health;
using Pickup;
using UnityEngine.SceneManagement;
using UnityEngine;
using Utility;

namespace Gameplay
{
    public class GameManager : Singleton<GameManager>
    {
        public uint Score { get; private set; } = 0;


        private uint _scoreToWin;
        
        private void Start()
        {
            SetupPlayerEvent();
            SetupCollectableEvent();
        }

        private void SetupPlayerEvent()
        {
            var player = GameObject.FindWithTag("Player");
            if (!player) return;
            if (!player.TryGetComponent(out CharacterHealth playerHealth)) return;
            playerHealth.OnDead.AddListener(ResetLevel);
        }

        private void SetupCollectableEvent()
        {
            var collectables = GameObject.FindGameObjectsWithTag("Collectable");
            _scoreToWin = (uint)collectables.Length;
            foreach (var collectable in collectables)
            {
                if (!collectable.TryGetComponent(out Collectable col)) return;
                col.OnCollect.AddListener(ScoreChangeHandler);
            }
        }

        private void ScoreChangeHandler()
        {
            Score += 1;
            if (_scoreToWin > Score) 
                Debug.Log($"Current Score : {Score}/{_scoreToWin}");
            else
            {
                Debug.Log($"Victory!");
                ResetLevel();
            }
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

