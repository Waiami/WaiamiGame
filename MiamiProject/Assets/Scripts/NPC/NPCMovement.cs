using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {
    [SerializeField] private float movementSpeed;
    [SerializeField] private float destinationRadius;
    [SerializeField] private float changeDestinationDelay;
    [SerializeField] private float minimumStandingTime;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject konfettiEffect;
    private bool destinationReached = false;
    private float changeDestinationCountDown;
    private float standingTimer;
    private WayPoint _wayPoint;
    private WayPoint _oldWayPoint;
    private bool dead;
    private Animator npcanimator;
    private SpriteRenderer npcSpriteRenderer;
    public SpriteRenderer npcHeadSpriteRenderer;

	// Use this for initialization
	void Start () {
        npcanimator = GetComponent<Animator>();
        npcSpriteRenderer = GetComponent<SpriteRenderer>();
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
                    npcanimator.SetTrigger("Idle");
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
        npcanimator.SetTrigger("Walk");
        Vector3 desiredPosition = _wayPoint.transform.position;
        Vector3 smoothPosition = Vector3.MoveTowards(transform.position, desiredPosition, movementSpeed * Time.deltaTime);
        if(desiredPosition.x <= transform.position.x)
        {
            npcSpriteRenderer.flipX = true;
            npcHeadSpriteRenderer.flipX = false;
        }
        else
        {
            npcSpriteRenderer.flipX = false;
            npcHeadSpriteRenderer.flipX = true;
        }
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
            npcanimator.SetTrigger("Idle");
        }
    }

    public void NPCKilled()
    {
        npcanimator.SetTrigger("Dead");
        GameObject effect1 = Instantiate(explosionEffect, npcanimator.transform);
        effect1.transform.SetParent(null);
        GameObject effect2 = Instantiate(konfettiEffect, npcHeadSpriteRenderer.transform);
        effect2.transform.SetParent(null);
        Destroy(npcHeadSpriteRenderer.gameObject);
        //Todo Add Kill Animations
        dead = true;
    }
}
