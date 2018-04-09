using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {

    [System.Serializable]
    public class Pool
    {
        [SerializeField]
        private string tag;
        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private int size;

        public string Tag { get { return tag; } }
        public GameObject Prefab { get { return prefab; } }
        public int Size { get { return size; } }
    }

#region singelton
    public static ProjectilePool Instance;

    private void Awake()
    {
        Instance = this;
    }
#endregion

    [SerializeField]
    private List<Pool> pools;
    [SerializeField]
    private Dictionary<string, Queue<GameObject>> poolDictionary;
	
    // Use this for initialization
	void Start () {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.Tag, objectPool);
           
        }
	}

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {

            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
	

}
