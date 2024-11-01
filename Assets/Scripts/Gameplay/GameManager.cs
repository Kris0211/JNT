using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameplaySettings gameplaySettings;

    [Header("Object References")]
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private Spawner _spawner;
    [SerializeField]
    private GameplayUI _gameplayUI;
    [SerializeField]
    private PauseMenu _pauseMenu;
    [SerializeField]
    private VictoryScreen _victoryScreen;

    private Transform _playerTransform;

    private int _collectedCoins;
    private int _colorChangesRemaining;

    private int _elapsedTime;
    private bool _isTimerActive = true;

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
        _playerTransform = _player.transform;

        if (_spawner == null)
        {
            GameObject spawner = GameObject.Find("CoinSpawner");
            if (spawner != null)
            {
                _spawner = spawner.GetComponent<Spawner>();
            }
        }
        Assert.IsNotNull(_spawner);
        _spawner.spawnDelay = gameplaySettings.coinSpawnInterval;
        _spawner.newObjectSpawned.AddListener(OnObjectSpawned);

        if (_gameplayUI == null)
        {
            GameObject ui = GameObject.Find("GameplayUI");
            if (ui != null)
            {
                _gameplayUI = ui.GetComponent<GameplayUI>();
            }
        }
        Assert.IsNotNull(_gameplayUI);
        _gameplayUI.MovementRequested += OnMovementButtonPressed;
        _gameplayUI.ColorChangeRequested += OnColorChanged;
        _gameplayUI.PauseGameRequested += OnGamePaused;
        // Refresh coin counter.
        _gameplayUI.UpdateCoinCounter(_collectedCoins, gameplaySettings.requiredCoins);

        if (_pauseMenu == null)
        {
            GameObject pauseMenu = GameObject.Find("PauseUI");
            if (pauseMenu != null)
            {
                _pauseMenu = pauseMenu.GetComponent<PauseMenu>();
            }
        }
        Assert.IsNotNull(_pauseMenu);
        _pauseMenu.GameResumed += OnGameResumed;
        _pauseMenu.GameRestarted += OnGameRestarted;
        _pauseMenu.GameQuit += OnGameQuit;

        if (_victoryScreen == null)
        {
            GameObject victoryScreen = GameObject.Find("VictoryScreen");
            if (victoryScreen != null)
            {
                _victoryScreen = victoryScreen.GetComponent<VictoryScreen>();
            }
        }
        Assert.IsNotNull(_victoryScreen);
        _victoryScreen.RestartRequested += OnGameRestarted;
        _victoryScreen.GameExited += OnGameQuit;
    }

    void Start()
    {
        StartCoroutine(MeasureTime());
    }

    public void OnObjectSpawned(GameObject obj)
    {
        if (obj.TryGetComponent<Coin>(out var coin))
        {
            coin.pickedUp.AddListener(OnCoinPickedUp);
        }
        _player.targetIndicator.UpdateTarget(FindNearestCoin());
    }

    public void OnCoinPickedUp(GameObject obj)
    {
        if (obj.TryGetComponent<Coin>(out var coin))
        {
            coin.pickedUp.RemoveAllListeners();
        }
        _spawner.DespawnObject(obj);

        _collectedCoins++;
        _colorChangesRemaining++;
        
        int _requiredCoins = gameplaySettings.requiredCoins;

        _gameplayUI.SetColorChangeEnabled(true);
        _gameplayUI.UpdateCoinCounter(_collectedCoins, _requiredCoins);

        _player.targetIndicator.UpdateTarget(FindNearestCoin());

        // Victory condition
        if (_collectedCoins >= _requiredCoins)
        {
            _isTimerActive = false;
            _spawner.ShouldSpawnObjects = false;
            gameEnded?.Invoke();
        }
    }

    public void OnMovementButtonPressed(Vector2 dir)
    {
        _player.Move(dir);
        _player.targetIndicator.UpdateTarget(FindNearestCoin());
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnGameQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator MeasureTime()
    {
        while (_isTimerActive)
        {
            yield return new WaitForSeconds(1f);
            _gameplayUI.UpdateTimer(++_elapsedTime);
        }
    }

    private Transform FindNearestCoin()
    {
        List<GameObject> activeCoins = _spawner.GetActiveCoins();
        if (activeCoins.Count == 0)
        {
            return null;
        }
        if (activeCoins.Count == 1)
        { 
            return activeCoins[0].transform;
        }

        // If there are 2 or more coins, find nearest one.
        Transform nearestCoin = null;
        Vector3 playerPos = _playerTransform.position;
        float minDistSqrt = Mathf.Infinity;

        foreach (GameObject coin in activeCoins)
        {
            if (!coin.activeInHierarchy) continue; // In case we somehow got an inactive coin, skip it.

            float distSqrt = (coin.transform.position - playerPos).sqrMagnitude;
            if (distSqrt < minDistSqrt)
            {
                minDistSqrt = distSqrt;
                nearestCoin = coin.transform;
            }
        }

        return nearestCoin;
    }
}
