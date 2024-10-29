using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly List<GameObject> _pool;
    private readonly GameObject _owner;
    private readonly GameObject _prefab;

    public GameObjectPool(GameObject owner, GameObject prefab, int size = 20) 
    {
        _owner = owner;
        _prefab = prefab;
        _pool = new List<GameObject>();
        
        for(int i = 0; i < size; i++)
        {
            GameObject go = Object.Instantiate(_prefab);
            go.SetActive(false);
            _pool.Add(go);
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
        GameObject newObject = Object.Instantiate(_prefab);
        newObject.SetActive(false);
        newObject.transform.SetParent(_owner.transform); // Reparent spawned objects to reduce clutter in scene.
        _pool.Add(newObject);
        return newObject;
    }

    public void ReleaseObject(GameObject go)
    {
        go.SetActive(false);
    }
}
