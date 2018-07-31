using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSprite : MonoBehaviour {

    public Sprite[] lootSprites;

	// Use this for initialization
	void Start () {
        int i = Random.Range(0, lootSprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = lootSprites[i];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
