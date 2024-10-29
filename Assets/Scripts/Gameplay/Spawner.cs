using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("Spawnable Object")]
    public GameObject objectToSpawn;
    public float objectSize = 0.64f;
    [Tooltip("How many objects should be spawned on game start.")]
    [SerializeField]
    private uint _poolSize = 10;

    [Header("Playable Area")]
    public Tilemap tilemap;
    [Tooltip("Shrinks playable area by given value to account for level bounds.")]
    public float playableAreaOffset = 0.64f;

    [Header("Events")]
    public UnityEvent<GameObject> newObjectSpawned;
       
    [HideInInspector]
    public float spawnDelay;

    private Vector3 _playableAreaMin = new();
    private Vector3 _playableAreaMax = new();

    private GameObjectPool _goPool;
    public bool ShouldSpawnObjects { private get; set; } = true;

    void Awake()
    {
        if (tilemap == null)
        {
            GameObject _tilemap = GameObject.Find("TileMap");
            if (_tilemap != null)
            {
                tilemap = _tilemap.GetComponent<Tilemap>();
            }
        }
        Assert.IsNotNull(tilemap);

        if (objectToSpawn == null)
        {
            Debug.LogError("Spawner: objectToSpawn is null. Disabling!");
            ShouldSpawnObjects = false;
        }
    }
    
    IEnumerator Start()
    {
        _goPool = new(gameObject, objectToSpawn, _poolSize); // Ensure _poolSize is not negative.

        CalculatePlayableArea();
        while(ShouldSpawnObjects)
        {
            GameObject spawnedObject = SpawnObject();
            newObjectSpawned?.Invoke(spawnedObject);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void DespawnObject(GameObject go)
    {
        _goPool.ReleaseObject(go);
    }

    public List<GameObject> GetActiveCoins()
    {
        return _goPool.GetActiveObjectsInPool();
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

        GameObject spawnedObject = _goPool.GetObject();
        spawnedObject.transform.position = spawnPos;
        return spawnedObject;
    }
} 
