using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int requiredCoins = 5;

    [SerializeField]
    private PlayerController _playerReference;
    [SerializeField]
    private Spawner _spawnerReference;

    private int _collectedCoins;
    private int _colorChangesRemaining;

    [Header("Events")]
    public UnityEvent gameWon;

    void Awake()
    {
        if (_playerReference == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _playerReference = player.GetComponent<PlayerController>();
            }
        }
        Assert.IsNotNull(_playerReference);

        Assert.IsNotNull(_spawnerReference); //todo: find spawner in scene
        _spawnerReference.newObjectSpawned.AddListener(OnObjectSpawned);
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnColorChange();
        }
    }

    public void OnObjectSpawned(GameObject obj)
    {
        Coin coin = obj.GetComponent<Coin>();
        if (coin != null)
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

    public void OnColorChange()
    {
        if (_colorChangesRemaining > 0)
        {
            _playerReference.ChangeColor();
            _colorChangesRemaining--;
        }
        else
        {
            Debug.Log("No changes remaining.");
        }
    }

}
