using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{

    public event Action RestartRequested;
    public event Action GameExited;

    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _quitButton;

    void Awake()
    {
        _restartButton.onClick.AddListener(() => RestartRequested?.Invoke());
        _quitButton.onClick.AddListener(() => GameExited?.Invoke());
    }
}
