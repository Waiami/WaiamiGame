using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WayPoint : MonoBehaviour {
    public List<Collider2D> nextWaypoints;
    public LayerMask layerMask;

    // Use this for initialization
    void Start () {
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position,6f);
        foreach(Collider2D c2d in hitCollider)
        {
            WayPoint wp = c2d.GetComponent<WayPoint>();
            if(wp != null && c2d.transform != this.transform)
            {
                if (!IsWallBetween(this.transform.position,c2d.transform.position))
                {
                    nextWaypoints.Add(c2d);
                }
            }
        }
	}
	
	public WayPoint SetDestinationWaypoint(WayPoint oldWaypoint)
    {
        System.Random rnd = new System.Random();
        var result = nextWaypoints.OrderBy(item => rnd.Next());
        WayPoint wp = result.First().GetComponent<WayPoint>();
        if (wp == oldWaypoint)
        {
            wp = result.Last().GetComponent<WayPoint>();
        }
        return wp;
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
