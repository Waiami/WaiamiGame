using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour {
    [SerializeField] private float delay = 1;
    private float timer;
	// Use this for initialization

    void Start () {
        timer = delay;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer < 0)
        {
            transform.SetParent(null);
            Destroy(this.transform.gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}
