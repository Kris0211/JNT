using System;
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
        if (_wobblingText != null) // Unity objects should not use null propagation.
        {
            _wobblingText.StopTweens();
        }        
        RestartRequested?.Invoke();
    }

    private void OnQuitButtonClicked()
    {
        if (_wobblingText != null)
        {
            _wobblingText.StopTweens();
        }
        GameExited?.Invoke();
    }
}
