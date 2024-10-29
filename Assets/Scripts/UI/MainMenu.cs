using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _quitButton;

    void Awake()
    {
        _playButton.onClick.AddListener(StartGame);
        //_settingsButton.onClick.AddListener()
        _quitButton.onClick.AddListener(() => Application.Quit());
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
