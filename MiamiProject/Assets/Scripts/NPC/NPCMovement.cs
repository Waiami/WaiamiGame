using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {
    [SerializeField] private float movementSpeed;
    [SerializeField] private float destinationRadius;
    [SerializeField] private float changeDestinationDelay;
    [SerializeField] private float minimumStandingTime;
    private bool destinationReached = false;
    private float changeDestinationCountDown;
    private float standingTimer;
    private WayPoint _wayPoint;
    private WayPoint _oldWayPoint;
    private bool dead;
	// Use this for initialization
	void Start () {
        changeDestinationCountDown = changeDestinationDelay;
        standingTimer = -1;
        dead = false;
	}

    public void SetMovementData(float movementSpeed, float destinationRadius, float changeDestinationDelay, WayPoint waypoint)
    {
        _wayPoint = waypoint;
        this.movementSpeed = movementSpeed;
        this.destinationRadius = destinationRadius;
        this.changeDestinationDelay = changeDestinationDelay;
    }
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            return;
        }

        if (!destinationReached)
        {
            if (_wayPoint == null)
            {
                return;
            }
            MoveToDestination();
            CheckDestinationArrived();
            CheckDestinationCountDown();
        }
        else
        {
            if (_wayPoint == null)
            {
                return;
            }
            if(standingTimer <= 0)
            {
                int i = Random.Range(0, 100);
                if(i >= 50)
                {
                    SetDestination();
                }
                else
                {
                    standingTimer = minimumStandingTime;
                }
            }
            else
            {
                standingTimer -= Time.deltaTime;
            }
            
        }
	}

    private void SetDestination()
    {
        _oldWayPoint = _wayPoint;
        _wayPoint = _wayPoint.SetDestinationWaypoint(_oldWayPoint);
        destinationReached = false;
        changeDestinationCountDown = changeDestinationDelay;
    }

    private void MoveToDestination()
    {
        Vector3 desiredPosition = _wayPoint.transform.position;
        Vector3 smoothPosition = Vector3.MoveTowards(transform.position, desiredPosition, movementSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }

    private void CheckDestinationCountDown()
    {
        if(changeDestinationCountDown < 0 && !destinationReached)
        {
            WayPoint wp = _wayPoint;
            _wayPoint = _oldWayPoint;
            _oldWayPoint = wp;
            changeDestinationCountDown = changeDestinationDelay;
        }
        else
        {
            changeDestinationCountDown -= Time.deltaTime;
        }
    }

    private void CheckDestinationArrived()
    {
        float x = Mathf.Abs(transform.position.x - _wayPoint.transform.position.x);
        float y = Mathf.Abs(transform.position.y - _wayPoint.transform.position.y);
        if (x <= destinationRadius && y <= destinationRadius)
        {
            destinationReached = true;
        }
    }

    public void NPCKilled()
    {
        dead = true;
    }
}
