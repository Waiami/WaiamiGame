using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHead : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetRandomHead();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetRandomHead()
    {
        Sprite[] newHead = GameStats.Instance.NPCheads;
        GetComponent<SpriteRenderer>().sprite = newHead[Random.Range(0, newHead.Length)];
    }
}
