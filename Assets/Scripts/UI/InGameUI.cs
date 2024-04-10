using Gameplay;
using Health;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusUI;
        [SerializeField] private TextMeshProUGUI _resultUI;
        private uint _healthCache;
        private uint _scoreCache;

        private void Awake()
        {
            var gameManager = GameObject.FindWithTag("GameController");
            if (!gameManager) return;
            var gm = gameManager.GetComponent<GameManager>();
            if (!gm) return;
            gm.OnScoreChange.AddListener(UpdateScoreText);
            
            gm.OnGameEnd.AddListener(GameEndHandler);
            
            var player = GameObject.FindWithTag("Player");
            if (!player) return;
            var health = player.GetComponent<CharacterHealth>();
            if (!health) return;
            health.OnHealthChange.AddListener(UpdateHealthText);

            _resultUI.text = "";
            UpdateText();
        }

        private void GameEndHandler(bool isVictory)
        {
            var resultText = isVictory ? "Victory!" : "The End!";

            _statusUI.text = "";
            _resultUI.text = resultText;
        }

        private void UpdateHealthText(uint health)
        {
            _healthCache = health;
            UpdateText();
        }

        private void UpdateScoreText(uint score)
        {
            _scoreCache = score;
            UpdateText();
        }

        private void UpdateText()
        {
            _statusUI.text = $"HP : {_healthCache}\nPT : {_scoreCache}";
        }
    }
}