using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool sharedInstance;
    public Pools[] pools;
    private Dictionary<int,GameObject[]> pooledObjects;
    public Transform environmentVariables;
    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new Dictionary<int, GameObject[]>();

        for (var i = 0; i < pools.Length; i++)
        {
            GameObject[] p = new GameObject[pools[i].amountToPool];
            
            for (int j = 0; j < pools[i].amountToPool; j++)
            {
                var tmp = Instantiate(pools[i].objectToPool, environmentVariables.transform, true);

                tmp.SetActive(false);

                p[j] = tmp;
            }
            
            pooledObjects.Add(i,p);
        }
    }

    public GameObject GetPooledObject(int i)
    {
        
        var p = pooledObjects[i];
        
        for (var j = 0; j < pools[i].amountToPool; j++)
        {
            if (!p[j].activeInHierarchy)
            {
                return p[j];
            }
        }

        return null;
    }
    
    [System.Serializable]
    public struct Pools
    {
        public GameObject objectToPool;
        public int amountToPool;
    }
}
