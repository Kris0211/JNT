using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public float spawnDelay = 3f;
    [Header("Spawnable Object")]
    public GameObject objectToSpawn;
    public float objectSize = 0.64f;

    [Header("Playable Area")]
    public Tilemap tilemap;

    private Vector3 _playableAreaMin = new();
    private Vector3 _playableAreaMax = new();

    private List<GameObject> _spawnedObjectReferences = new();

    IEnumerator Start()
    {
        CalculatePlayableArea();
        while (true) 
        {
            _spawnedObjectReferences.Add(SpawnObject());
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void CalculatePlayableArea()
    {
        BoundsInt tilemapBounds = tilemap.cellBounds;

        Vector3Int minCellPos = tilemapBounds.min + new Vector3Int(1, 1, 0); // Offset by 1 tile to exclude walls
        Vector3Int maxCellPos = tilemapBounds.max - new Vector3Int(1, 1, 0);

        _playableAreaMin = tilemap.CellToWorld(minCellPos);
        _playableAreaMax = tilemap.CellToWorld(maxCellPos);

        Debug.Log("Playable Area Min: " + _playableAreaMin);
        Debug.Log("Playable Area Max: " + _playableAreaMax);
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
