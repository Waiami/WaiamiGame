using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCGenerator : MonoBehaviour {
    [SerializeField] private Transform npcParent;
    [SerializeField] private int minNPCs;
    [SerializeField] private int maxNPCs;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform wayPointParent;
    public Transform[] waypoints;
	// Use this for initialization
	void Start () {
        waypoints =  wayPointParent.GetComponentsInChildren<Transform>();
        SpawnNPCs(npcPrefab, waypoints, minNPCs, maxNPCs);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SpawnNPCs(GameObject spawnObject, Transform[] waypoints, int minNPCs, int maxNPCs)
    {
        int max = Random.Range(minNPCs, maxNPCs);
        int count = waypoints.Length;
        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, count).OrderBy(x => rnd.Next()).Take(max).ToList();

        for (int i = 0; i < max; i++)
        {
            Transform position = waypoints[randomNumbers[i]];
            if (position != null)
            {
                GameObject obj = Instantiate(spawnObject, position);
                float movementspeed = Random.Range(GameStats.Instance.MinMovementSpeed, GameStats.Instance.MaxMovementSpeed);
                float destinationRadius = Random.Range(GameStats.Instance.MinDesinationRadius, GameStats.Instance.MaxDestinationRadius);
                float changeDestinationDelay = Random.Range(GameStats.Instance.MinChangeDestinationDelay, GameStats.Instance.MaxChangeDestinationDelay);
                obj.GetComponent<NPCMovement>().SetMovementData(movementspeed, destinationRadius, changeDestinationDelay, position.GetComponent<WayPoint>());
                obj.transform.SetParent(npcParent);
                
            }

        }
    }

    #region Instance
    public static NPCGenerator Instance;

    void Awake()
    {
        if (Instance != null)
        {
            if (this != Instance)
            {
                Destroy(this);
            }
        }
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }


    }
    #endregion
}
