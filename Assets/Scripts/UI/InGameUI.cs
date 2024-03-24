using Gameplay;
using Health;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;
        private uint _healthCache;
        private uint _scoreCache;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();

            var gameManager = GameObject.FindWithTag("GameController");
            if (!gameManager) return;
            var gm = gameManager.GetComponent<GameManager>();
            if (!gm) return;
            gm.OnScoreChange.AddListener(UpdateScoreText);
            
            var player = GameObject.FindWithTag("Player");
            if (!player) return;
            var health = player.GetComponent<CharacterHealth>();
            if (!health) return;
            health.OnHealthChange.AddListener(UpdateHealthText);
            
            UpdateText();
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
            _textMeshPro.text = $"HP : {_healthCache}\nPT : {_scoreCache}";
        }
    }
}