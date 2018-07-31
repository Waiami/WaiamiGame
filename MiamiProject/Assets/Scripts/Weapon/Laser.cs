using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private PlayerDataModel referencePlayerDataModel;
    private LineRenderer lineRenderer;
    private float laserDelay = 0.5f;
    [SerializeField] private float laserTimer;
    public Transform laserHit;
    public LayerMask layermask;
    [SerializeField] public RaycastHit2D[] hits;

    public PlayerDataModel ReferencePlayerDataModel { get { return referencePlayerDataModel; } }


    // Use this for initialization
    void Start () {
        laserTimer = laserDelay;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        laserHit.transform.SetParent(null);
	}
	
	// Update is called once per frame
	void Update () {
        if(laserTimer > 0)
        {
            laserTimer -= Time.deltaTime;
            ShootLaser();
        }
        else
        {
            Destroy(laserHit.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void ShootLaser()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(transform.position, transform.up, 100, layermask);
        laserHit.position = hit2d.point;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserHit.position);
        CheckForHits();
    }

    private void CheckForHits()
    {
        hits = Physics2D.RaycastAll(transform.position, transform.up, Vector2.Distance(transform.position, laserHit.position));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.tag == "Player" && referencePlayerDataModel != hit.transform.GetComponent<PlayerDataModel>() && hit.transform.GetComponent<PlayerDataModel>().IsDead == false)
            {
                referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(hit.transform.GetComponent<PlayerDataModel>().PointsWorth);
                hit.transform.GetComponent<PlayerController>().KillPlayer();
            }
            if (hit.transform.GetComponent<NPC>() != null)
            {
                if (hit.transform.GetComponent<NPC>().Dead == false)
                {
                    referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(GameStats.Instance.PointsForNPCS);
                    hit.transform.GetComponent<NPC>().KillNPC();
                }
            }
        }
    }


    public void SetPlayerDataModel(PlayerDataModel value)
    {
        referencePlayerDataModel = value;
    }
}
