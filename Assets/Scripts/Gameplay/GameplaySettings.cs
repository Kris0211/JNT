using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "ScriptableObjects/GameplaySettings")]
public class GameplaySettings : ScriptableObject
{
    public int requiredCoins = 5;
    public float coinSpawnInterval = 3;

    public void SetValues(int requiredCoins, float coinSpawnInterval)
    {
        this.requiredCoins = requiredCoins;
        this.coinSpawnInterval = coinSpawnInterval;
    }
}