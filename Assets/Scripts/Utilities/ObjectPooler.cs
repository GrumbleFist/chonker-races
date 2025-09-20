using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ChonkerRaces.Utilities
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
            public bool expandable = true;
        }
        
        [Header("Pool Settings")]
        [SerializeField] private List<Pool> pools = new List<Pool>();
        [SerializeField] private bool createOnStart = true;
        [SerializeField] private Transform poolParent;
        
        private Dictionary<string, Queue<GameObject>> poolDictionary;
        private Dictionary<string, Pool> poolInfo;
        
        public static ObjectPooler Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializePooler();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            if (createOnStart)
            {
                CreatePools();
            }
        }
        
        private void InitializePooler()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            poolInfo = new Dictionary<string, Pool>();
            
            // Create pool parent if not assigned
            if (poolParent == null)
            {
                GameObject parent = new GameObject("Pooled Objects");
                poolParent = parent.transform;
                poolParent.SetParent(transform);
            }
            
            // Build pool info dictionary
            foreach (Pool pool in pools)
            {
                poolInfo[pool.tag] = pool;
            }
        }
        
        public void CreatePools()
        {
            foreach (Pool pool in pools)
            {
                CreatePool(pool);
            }
        }
        
        private void CreatePool(Pool pool)
        {
            if (pool.prefab == null)
            {
                Debug.LogError($"Pool '{pool.tag}' has no prefab assigned!");
                return;
            }
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary[pool.tag] = objectPool;
            Debug.Log($"Created pool '{pool.tag}' with {pool.size} objects");
        }
        
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogError($"Pool with tag '{tag}' doesn't exist!");
                return null;
            }
            
            GameObject objectToSpawn = null;
            
            // Try to get an object from the pool
            if (poolDictionary[tag].Count > 0)
            {
                objectToSpawn = poolDictionary[tag].Dequeue();
            }
            else if (poolInfo[tag].expandable)
            {
                // Create a new object if pool is expandable
                objectToSpawn = Instantiate(poolInfo[tag].prefab, poolParent);
                Debug.Log($"Expanded pool '{tag}' with new object");
            }
            else
            {
                Debug.LogWarning($"Pool '{tag}' is empty and not expandable!");
                return null;
            }
            
            // Setup the object
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            
            // Call OnObjectSpawned if the object has the interface
            IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
            if (pooledObject != null)
            {
                pooledObject.OnObjectSpawned();
            }
            
            return objectToSpawn;
        }
        
        public void ReturnToPool(string tag, GameObject obj)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogError($"Pool with tag '{tag}' doesn't exist!");
                return;
            }
            
            // Call OnObjectReturned if the object has the interface
            IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            if (pooledObject != null)
            {
                pooledObject.OnObjectReturned();
            }
            
            // Reset the object
            obj.SetActive(false);
            obj.transform.SetParent(poolParent);
            
            // Return to pool
            poolDictionary[tag].Enqueue(obj);
        }
        
        public void ReturnToPoolAfterDelay(string tag, GameObject obj, float delay)
        {
            StartCoroutine(ReturnToPoolCoroutine(tag, obj, delay));
        }
        
        private IEnumerator ReturnToPoolCoroutine(string tag, GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnToPool(tag, obj);
        }
        
        // Utility methods
        public void ClearPool(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogError($"Pool with tag '{tag}' doesn't exist!");
                return;
            }
            
            Queue<GameObject> pool = poolDictionary[tag];
            
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            
            Debug.Log($"Cleared pool '{tag}'");
        }
        
        public void ClearAllPools()
        {
            foreach (string tag in poolDictionary.Keys)
            {
                ClearPool(tag);
            }
        }
        
        public void ExpandPool(string tag, int additionalSize)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogError($"Pool with tag '{tag}' doesn't exist!");
                return;
            }
            
            Pool pool = poolInfo[tag];
            
            for (int i = 0; i < additionalSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent);
                obj.SetActive(false);
                poolDictionary[tag].Enqueue(obj);
            }
            
            Debug.Log($"Expanded pool '{tag}' by {additionalSize} objects");
        }
        
        // Getters
        public int GetPoolSize(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                return 0;
            }
            
            return poolDictionary[tag].Count;
        }
        
        public int GetActivePoolSize(string tag)
        {
            if (!poolInfo.ContainsKey(tag))
            {
                return 0;
            }
            
            return poolInfo[tag].size - GetPoolSize(tag);
        }
        
        public bool PoolExists(string tag)
        {
            return poolDictionary.ContainsKey(tag);
        }
        
        public List<string> GetPoolTags()
        {
            return new List<string>(poolDictionary.Keys);
        }
        
        // Editor methods
        [ContextMenu("Create All Pools")]
        public void CreateAllPools()
        {
            CreatePools();
        }
        
        [ContextMenu("Clear All Pools")]
        public void ClearAllPoolsEditor()
        {
            ClearAllPools();
        }
    }
    
    // Interface for pooled objects
    public interface IPooledObject
    {
        void OnObjectSpawned();
        void OnObjectReturned();
    }
}
