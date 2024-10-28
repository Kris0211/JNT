using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public event Action GameResumed;
    public event Action GameRestarted;
    public event Action GameQuit;

    [SerializeField]
    private Button _resumeButton;
    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _quitButton;

    void Start()
    {
        _resumeButton.onClick.AddListener(() => GameResumed?.Invoke());
        _restartButton.onClick.AddListener(() => GameRestarted?.Invoke());
        _quitButton.onClick.AddListener(() => GameQuit?.Invoke());
    }
}
