using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummiRunning : MonoBehaviour {
    public float speed = 5;
    public Transform startposition;
    public Transform endposition;
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        float x = speed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + x, transform.position.y);
        if(transform.position.x > endposition.position.x)
        {
            transform.position = startposition.position;
        }
	}
}
