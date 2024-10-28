using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int requiredCoins = 5;

    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private Spawner _spawner;
    [SerializeField]
    private UIController _gameplayUI;

    private int _collectedCoins;
    private int _colorChangesRemaining;

    [Header("Events")]
    public UnityEvent gameWon;

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
        _gameplayUI.movementRequested.AddListener(OnMovementButtonPressed);
        _gameplayUI.colorChangeRequested.AddListener(OnColorChanged);
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

        if (_collectedCoins >= requiredCoins)
        {
            gameWon?.Invoke();
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
        else
        {
            Debug.Log("No changes remaining.");
        }
    }
}
