﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private float lifetime = 0.5f;
    [SerializeField]
    private float moveSpeed = 0.5f;

    private string playercode;

    private bool noDamage;

	// Use this for initialization
	void Start () {
        noDamage = false;
	}
	
	// Update is called once per frame
	void Update () {
        CheckLiveTime();
        var z = moveSpeed * Time.deltaTime;
        transform.Translate(0, z, 0);
    }

    void CheckLiveTime()
    {
        if(lifetime < 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    }

    public void SetPlayerCode(string value)
    {
        playercode = value;
        this.transform.name = this.transform.name + value;
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name + " " + collision.tag);
        if (collision.tag == "Wall")
        {
            noDamage = true;
            Destroy(this.gameObject);
            Debug.Log("Wall");
        }
        if (collision.tag == "Weapon")
        {
            noDamage = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}