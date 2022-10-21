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

        public int GetObjectID()
        {
            return objectName.GetHashCode();
        }
    }

    PoolObject objectFromPool;
    public GameObject GetObjectOfTypeFromPool(int objectID, Vector3 position, Quaternion rotation)
    {
        if (objectID < 0 || objectID >= pools.Length)
        {
            Debug.LogWarning("Invalid Pool Object ID");
            return null;
        }
        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].GetObjectID() == objectID)
            {
                objectFromPool = pools[i].GetNextInQueue();
                objectFromPool.transform.position = position;
                objectFromPool.transform.rotation = rotation;
                objectFromPool.gameObject.SetActive(true);
                pools[i].PlaceInQueue(objectFromPool);
                return objectFromPool.gameObject;
            }
        }
        return null;
    }

    public GameObject GetObjectFromPoolWithLifeTime(int objectID, Vector3 position, Quaternion rotation, float lifeTime)
    {
        if (objectID < 0 || objectID >= pools.Length)
        {
            Debug.LogWarning("Invalid Pool Object ID");
            return null;
        }
        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].GetObjectID() == objectID)
            {
                objectFromPool = pools[i].GetNextInQueue();
                objectFromPool.lifeTime = lifeTime;
                objectFromPool.transform.position = position;
                objectFromPool.transform.rotation = rotation;
                objectFromPool.gameObject.SetActive(true);
                pools[i].PlaceInQueue(objectFromPool);
                return objectFromPool.gameObject;
            }
        }
        return null;
    }

    public GameObject GetObjectFromPoolWithLifeTime(int objectID, Vector3 position, Quaternion rotation, Vector3 scale, float lifeTime)
    {
        if (objectID < 0 || objectID >= pools.Length)
        {
            Debug.LogWarning("Invalid Pool Object ID");
            return null;
        }
        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].GetObjectID() == objectID)
            {
                objectFromPool = pools[i].GetNextInQueue();
                objectFromPool.lifeTime = lifeTime;
                objectFromPool.transform.position = position;
                objectFromPool.transform.rotation = rotation;
                objectFromPool.transform.localScale = scale;
                objectFromPool.gameObject.SetActive(true);
                pools[i].PlaceInQueue(objectFromPool);
                return objectFromPool.gameObject;
            }
        }
        return null;
    }

    public int GetPoolObjectID(string objectName)
    {
        return Array.FindIndex(pools, pool => pool.Name == objectName);
    }
}
