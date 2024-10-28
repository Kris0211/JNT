using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int requiredCoins = 5;

    [Header("Object References")]
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private Spawner _spawner;
    [SerializeField]
    private UIController _gameplayUI;
    [SerializeField]
    private PauseMenu _pauseMenu;

    private int _collectedCoins;
    private int _colorChangesRemaining;

    [Header("Events")]
    public UnityEvent gameEnded;

    void Awake()
    {
        if (_player == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _player = player.GetComponent<PlayerController>();
            }
        }
        Assert.IsNotNull(_player);

        Assert.IsNotNull(_spawner); //todo: find spawner in scene
        _spawner.newObjectSpawned.AddListener(OnObjectSpawned);

        Assert.IsNotNull(_gameplayUI);
        _gameplayUI.MovementRequested += OnMovementButtonPressed;
        _gameplayUI.ColorChangeRequested += OnColorChanged;
        _gameplayUI.PauseGameRequested += OnGamePaused;

        Assert.IsNotNull(_pauseMenu);
        _pauseMenu.GameResumed += OnGameResumed;
        _pauseMenu.GameRestarted += OnGameRestarted;
        _pauseMenu.GameQuit += OnGameQuit;
    }

    public void OnObjectSpawned(GameObject obj)
    {
        if (obj.TryGetComponent<Coin>(out var coin))
        {
            coin.pickedUp.AddListener(OnCoinPickedUp);
        }
    }

    public void OnCoinPickedUp()
    {
        _collectedCoins++;
        _colorChangesRemaining++;
        _gameplayUI.SetColorChangeEnabled(true);

        if (_collectedCoins >= requiredCoins)
        {
            gameEnded?.Invoke();
        }
    }

    public void OnMovementButtonPressed(Vector2 dir)
    {
        _player.Move(dir);
    }

    public void OnColorChanged()
    {
        if (_colorChangesRemaining > 0)
        {
            _player.ChangeColor();
            _colorChangesRemaining--;
        }
        if (_colorChangesRemaining == 0)
        {
            _gameplayUI.SetColorChangeEnabled(false);
        }
    }

    public void OnGamePaused()
    {
        _pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnGameResumed()
    {
        _pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnGameRestarted()
    {
        DOTween.KillAll();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnGameQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
