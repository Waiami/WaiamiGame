﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    [SerializeField] private Animator explosionAnimator;
    private PlayerDataModel referencePlayerDataModel;
	// Use this for initialization
	void Start () {
        if(explosionAnimator == null)
        {
            explosionAnimator = GetComponent<Animator>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (explosionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Any"))
        {
            Destroy(this.gameObject);
        }
	}
    public void SetReferencePlayerDataModel(PlayerDataModel value)
    {
        referencePlayerDataModel = value;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (referencePlayerDataModel != collision.gameObject.GetComponent<PlayerDataModel>())
            {
                referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(collision.gameObject.GetComponent<PlayerDataModel>().PointsWorth);
            }
            collision.gameObject.GetComponent<PlayerController>().KillPlayer();
        }
    }

}