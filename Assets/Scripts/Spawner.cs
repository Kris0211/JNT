using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [Space(8)]
    public float spawnDelay = 3f;
    
    [Header("Spawnable Object")]
    public GameObject objectToSpawn;
    public float objectSize = 0.64f;

    [Header("Playable Area")]
    public Tilemap tilemap;
    public float playableAreaOffset = 0.64f;

    [Header("Events")]
    public UnityEvent<GameObject> newObjectSpawned;

    private Vector3 _playableAreaMin = new();
    private Vector3 _playableAreaMax = new();

    public bool ShouldSpawnObjects { private get; set; } = true;
    
    IEnumerator Start()
    {
        CalculatePlayableArea();
        while(ShouldSpawnObjects) 
        {
            GameObject spawnedObject = SpawnObject();
            newObjectSpawned?.Invoke(spawnedObject);
            yield return new WaitForSeconds(spawnDelay);
        }
    }


    private void CalculatePlayableArea()
    {
        BoundsInt tilemapBounds = tilemap.cellBounds;

        Vector3 offset = new(playableAreaOffset, playableAreaOffset, 0);
        _playableAreaMin = tilemap.CellToWorld(tilemapBounds.min) + offset;
        _playableAreaMax = tilemap.CellToWorld(tilemapBounds.max) - offset;
    }

    private GameObject SpawnObject()
    {
        float offset = objectSize / 2;

        Vector3 spawnMin = new(_playableAreaMin.x + offset, _playableAreaMin.y + offset, 0);
        Vector3 spawnMax = new(_playableAreaMax.x - offset, _playableAreaMax.y - offset, 0);

        Vector3 spawnPos = new(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y),
            0
        );

        return Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
    }
} 
