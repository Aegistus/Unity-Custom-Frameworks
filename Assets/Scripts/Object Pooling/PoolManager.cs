using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    public Pool[] pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(this);
        }
        GameObject poolParent = new GameObject("PoolParent");
        GameObject obj;
        foreach (Pool pool in pools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                obj = Instantiate(pool.objectPrefab, Vector3.one * 1000, Quaternion.identity, poolParent.transform);
                PoolObject poolObj = obj.AddComponent<PoolObject>();
                poolObj.lifeTime = 10f;
                obj.SetActive(false);
                pool.PlaceInQueue(poolObj);
            }
        }
    }

    [System.Serializable]
    public class Pool
    {
        [SerializeField] string objectName;
        public string Name => objectName;
        public GameObject objectPrefab;
        public int size;
        private Queue<PoolObject> inPool = new Queue<PoolObject>();

        public PoolObject GetNextInQueue()
        {
            return inPool.Dequeue();
        }

        public void PlaceInQueue(PoolObject obj)
        {
            inPool.Enqueue(obj);
        }
    }

    PoolObject objectFromPool;
    /// <summary>
	/// Spawns a pool object with the given objectID at the given position and rotation.
    /// Logs a warning if the objectID is invalid.
	/// </summary>
	/// <returns> The pool object's GameObject. null if the objectID is invalid. </returns>
    public GameObject SpawnObject(int objectID, Vector3 position, Quaternion rotation)
    {
        if (objectID < 0 || objectID >= pools.Length)
        {
            Debug.LogWarning("Invalid Pool Object ID");
            return null;
        }
        objectFromPool = pools[objectID].GetNextInQueue();
        objectFromPool.transform.position = position;
        objectFromPool.transform.rotation = rotation;
        objectFromPool.gameObject.SetActive(true);
        pools[objectID].PlaceInQueue(objectFromPool);
        return objectFromPool.gameObject;
    }

    /// <summary>
	/// Spawns a pool object with the given objectID at the given position and rotation.
    /// Also sets the object's lifetime to lifeTime.
    /// Logs a warning if the objectID is invalid.
	/// </summary>
	/// <returns> The pool object's GameObject. null if the objectID is invalid. </returns>
    public GameObject SpawnObjectWithLifetime(int objectID, Vector3 position, Quaternion rotation, float lifeTime)
    {
        if (objectID < 0 || objectID >= pools.Length)
        {
            Debug.LogWarning("Invalid Pool Object ID");
            return null;
        }
        objectFromPool = pools[objectID].GetNextInQueue();
        objectFromPool.lifeTime = lifeTime;
        objectFromPool.transform.position = position;
        objectFromPool.transform.rotation = rotation;
        objectFromPool.gameObject.SetActive(true);
        pools[objectID].PlaceInQueue(objectFromPool);
        return objectFromPool.gameObject;
    }

    /// <summary>
	/// Spawns a pool object with the given objectID with the given position, rotation, and scale.
    /// Logs a warning if the objectID is invalid.
	/// </summary>
	/// <returns> The pool object's GameObject. null if the objectID is invalid. </returns>
    public GameObject SpawnObjectWithLifetime(int objectID, Vector3 position, Quaternion rotation, Vector3 scale, float lifeTime)
    {
        if (objectID < 0 || objectID >= pools.Length)
        {
            Debug.LogWarning("Invalid Pool Object ID");
            return null;
        }
        objectFromPool = pools[objectID].GetNextInQueue();
        objectFromPool.lifeTime = lifeTime;
        objectFromPool.transform.position = position;
        objectFromPool.transform.rotation = rotation;
        objectFromPool.transform.localScale = scale;
        objectFromPool.gameObject.SetActive(true);
        pools[objectID].PlaceInQueue(objectFromPool);
        return objectFromPool.gameObject;

    }

    /// <summary>
	/// Gets the objectID of a poolObject given it's name.
	/// </summary>
	/// <returns> The PoolObject's integer ID. -1 if not found. </returns>
    public int GetPoolObjectID(string objectName)
    {
        int id = Array.FindIndex(pools, pool => pool.Name == objectName);
		if (id == -1)
        {
			Debug.LogWarning("Pool Object with name: " + name + " does not exist!");
        }
		return id;
    }
}
