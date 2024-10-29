using System;
using UnityEngine;
using UnityEngine.Assertions;
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
        Assert.IsNotNull(_resumeButton);
        _resumeButton.onClick.AddListener(() => GameResumed?.Invoke());

        Assert.IsNotNull(_restartButton);
        _restartButton.onClick.AddListener(() => GameRestarted?.Invoke());

        Assert.IsNotNull(_quitButton);
        _quitButton.onClick.AddListener(() => GameQuit?.Invoke());
    }
}
