using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour {

    private PlayerDataModel referencePlayerDataModel;
    public LayerMask layerMask;
    [SerializeField] private float delay = 0.5f;
    private float timer;
    // Use this for initialization
    void Start()
    {
        timer = delay;
        transform.localScale = new Vector3(1f, 1f, 1);

    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
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
            if (!IsWallBetween(this.transform.position, collision.transform.position)&& collision.GetComponent<PlayerDataModel>() != referencePlayerDataModel && collision.GetComponent<PlayerDataModel>().IsDead == false)
            {
                if (referencePlayerDataModel != collision.gameObject.GetComponent<PlayerDataModel>())
                {
                    referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(collision.gameObject.GetComponent<PlayerDataModel>().PointsWorth);
                }
                collision.gameObject.GetComponent<PlayerController>().KillPlayer();
            }
        }
        if (collision.GetComponent<NPC>() != null)
        {
            if (collision.GetComponent<NPC>().Dead == false)
            {
                referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(GameStats.Instance.PointsForNPCS);
                collision.GetComponent<NPC>().KillNPC();
            }

        }
    }

    private bool IsWallBetween(Vector2 origin, Vector2 target)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, (target - origin), (origin - target).magnitude, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Wall")
            {
                return true;
            }
        }
        return false;
    }
}
