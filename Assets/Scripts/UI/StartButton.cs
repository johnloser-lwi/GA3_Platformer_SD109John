using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => GameSceneManager.Instance.LoadScene("TestLevel"));
        }
    }
}