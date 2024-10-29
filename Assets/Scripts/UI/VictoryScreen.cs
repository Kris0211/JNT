using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public event Action RestartRequested;
    public event Action GameExited;

    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _quitButton;
    
    [Space(4)]
    [SerializeField]
    private WobbleEffect _wobblingText;

    void Awake()
    {
        Assert.IsNotNull(_restartButton);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);

        Assert.IsNotNull(_quitButton);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        _wobblingText?.StopTweens();
        RestartRequested?.Invoke();
    }

    private void OnQuitButtonClicked()
    {
        _wobblingText?.StopTweens();
        GameExited?.Invoke();
    }
}
