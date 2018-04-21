using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawner : MonoBehaviour {

    [SerializeField] private GameObject pickUp;

	// Use this for initialization
	void Start () {

	}
	
	public void SpawnNewPickUps()
    {
        foreach(Transform child in transform)
        {
            if(child.childCount == 0)
                Instantiate(pickUp, child);
        }
    }
}
