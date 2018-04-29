using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private float lifetime = 0.5f;
    [SerializeField]
    private float moveSpeed = 0.5f;

    private string playercode;

    private bool noDamage;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        noDamage = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        var z = moveSpeed * Time.deltaTime;
        Vector2 movement = new Vector2(z, 0);
        rb2d.velocity = transform.up * moveSpeed;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckLiveTime();
        
    }

    void CheckLiveTime()
    {
        if(lifetime < 0)
        {
            this.gameObject.SetActive(false);
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
        if (collision.tag == "Wall")
        {
            noDamage = true;
            Destroy(this.gameObject);
        }
        if (collision.tag == "Weapon" || collision.tag == "Bullet_P1" || collision.tag == "Bullet_P2" || collision.tag == "Bullet_P3" || collision.tag == "Bullet_P4")
        {
            noDamage = true;
        }
        if(collision.tag == "Player")
        {
            if(this.tag != "Bullet_" + collision.gameObject.GetComponentInParent<PlayerDataModel>().PlayerCode)
            {
                collision.gameObject.GetComponent<PlayerDataModel>().KillPlayer();
                Destroy(this.gameObject);
            }
        }
    }
}
