﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private float lifetime = 0.5f;
    [SerializeField]
    private float moveSpeed = 0.5f;
    private PlayerDataModel referencePlayerDataModel;

    private bool noDamage;
    private Rigidbody2D rb2d;

    public PlayerDataModel ReferencePlayerDataModel { get { return referencePlayerDataModel; } }

	void Start () {
        noDamage = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.up * moveSpeed;
    }
	
	void FixedUpdate () {
        CheckLiveTime();
        
    }

    void CheckLiveTime()
    {
        if(lifetime < 0)
        {
            DestroyObject();
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    }

    public void SetPlayerDataModel(PlayerDataModel value)
    {
        referencePlayerDataModel = value;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Wall" || collision.tag == "Bullet")
        {
            noDamage = true;
            DestroyObject();
        }
        if(collision.tag == "Player")
        {
            if (referencePlayerDataModel != collision.gameObject.GetComponent<PlayerDataModel>())
            {
                if (!noDamage)
                {
                    referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(collision.gameObject.GetComponent<PlayerDataModel>().PointsWorth);
                    collision.gameObject.GetComponent<PlayerController>().KillPlayer();
                }
                DestroyObject();
            }
        }
        if(collision.GetComponent<NPC>() != null)
        {
            if (!noDamage && collision.GetComponent<NPC>().Dead == false)
            {
                referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(GameStats.Instance.PointsForNPCS);
                collision.GetComponent<NPC>().KillNPC();
            }
            DestroyObject();
        }
    }

    public virtual void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
