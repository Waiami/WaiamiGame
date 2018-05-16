using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    [SerializeField] private Animator explosionAnimator;
    private PlayerDataModel referencePlayerDataModel;
    public LayerMask layerMask;
	
    // Use this for initialization
	void Start () {
        if(explosionAnimator == null)
        {
            explosionAnimator = GetComponent<Animator>();
            
        }
        transform.localScale = new Vector3(4.9f, 4.9f , 1);

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
            if(!IsWallBetween(this.transform.position, collision.transform.position))
            {
                if (referencePlayerDataModel != collision.gameObject.GetComponent<PlayerDataModel>())
                {
                    referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(collision.gameObject.GetComponent<PlayerDataModel>().PointsWorth);
                }
                collision.gameObject.GetComponent<PlayerController>().KillPlayer();
            }
        }
    }

    private bool IsWallBetween(Vector2 origin, Vector2 target)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, (target - origin), (origin-target).magnitude, layerMask);
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Wall")
            {
                return true;
            }
        }
        return false;
    }

}
