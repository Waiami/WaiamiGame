using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    [SerializeField] private Sprite Deadsprite;
    private bool dead;
    public bool Dead { get { return dead; } }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KillNPC()
    {
        GetComponent<SpriteRenderer>().sprite = Deadsprite;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<NPCMovement>().NPCKilled();
        dead = true;
    }
}
