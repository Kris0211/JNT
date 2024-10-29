using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly List<GameObject> _pool;
    private readonly GameObject _owner;
    private readonly GameObject _prefab;

    public GameObjectPool(GameObject owner, GameObject prefab, uint size = 20) 
    {
        _owner = owner;
        _prefab = prefab;
        _pool = new List<GameObject>();
        
        for(uint i = 0; i < size; i++)
        {
            _ = CreateObject();
        }
    }

    public GameObject GetObject()
    {
        // Try to get object from pool
        foreach (GameObject go in _pool)
        {
            if (!go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        // If there are no more objects in pool, create a new one.
        GameObject newObject = CreateObject();
        newObject.SetActive(true);
        return newObject;
    }

    public void ReleaseObject(GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
    }

    public List<GameObject> GetActiveObjectsInPool()
    {
        List<GameObject> activeObjects = new();
        foreach (GameObject go in _pool)
        {
            if (go.activeInHierarchy)
            {
                activeObjects.Add(go);
            }
        }
        return activeObjects;
    }

    private GameObject CreateObject()
    {
        GameObject go = Object.Instantiate(_prefab);
        go.SetActive(false);
        go.transform.SetParent(_owner.transform); // Reparent spawned objects to reduce clutter in scene.
        _pool.Add(go);
        return go;
    }
}
