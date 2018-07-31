using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

#region variables
    
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private PlayerDataModel playerDataModel;
    [SerializeField] private PlayerSprite playerSprite;
    [SerializeField] private Transform headPoint;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Transform CameraPoint;
    [SerializeField] private CameraFollow playerCamera;
    [SerializeField] private GameObject SmokeEffect;

    public PlayerDataModel PlayerModelScript { get { return playerDataModel; } }

    private float attackCooldown = -1;

    [Header("GeneralWeapons")]
    [SerializeField] private GameObject weaponRotationObject;
    private GameObject ActiveWeaponObject;
    private Transform activeWeaponPoint;
    private List<GameObject> weaponObjects;

    [Header("Pistol")]
    [SerializeField]private Transform pistolPoint;
    [SerializeField] private GameObject pistolObject;

    [Header("Cannon")]
    [SerializeField]
    private Transform cannonPoint;
    [SerializeField] private GameObject cannonObject;

    [Header("Knife")]
    [SerializeField]
    private Transform knifePoint;
    [SerializeField] private GameObject knifeObject;

    [Header("Laser")]
    [SerializeField]
    private Transform laserPoint;
    [SerializeField] private GameObject laserObject;

    [Header("Snake")]
    [SerializeField]
    private Transform snakePoint;
    [SerializeField] private GameObject snakeObject;

    [Header("Rocket")]
    [SerializeField]
    private Transform rocketPoint;
    [SerializeField] private GameObject rocketObject;

    [Header("Dragon")]
    [SerializeField]
    private Transform dragonPoint;
    [SerializeField] private GameObject dragonObject;


    private bool hasPickUp = false;
    private GameObject pickUpObject;

    private bool shootIsInComming = false;
    private float activeFireDelay;
    [SerializeField]private bool cantMove;
    public bool CantMove { set { cantMove = value; } }

    private AudioSource sfxSource;
    #endregion

    // Use this for initialization
    void Start () {
        Initialize();
        SetHeadState(false);
        StartCoroutine(WaitActivateHead());
        playerDataModel.AddPoints(GameStats.Instance.PlayerStartScore);
        //playerUI.SetPointText(playerDataModel.PlayerScore);
        playerCamera.GetComponent<PlayerCanvasBehaviour>().PopUpScore("", playerDataModel.PlayerScore.ToString());
        playerAnimator.InstantiateSpawnEffects(transform);
    }

    private void FixedUpdate()
    {
        UpdateAttackCooldown();
        if (shootIsInComming)
        {
            if (activeFireDelay < 0)
            {
                shootIsInComming = false;
                sfxSource.Stop();
                ShootWeapon();
            }
        }
    }

    void Initialize()
    {
        if(playerSprite == null)
        {
            playerSprite = gameObject.GetComponent<PlayerSprite>();
        }
        if(playerDataModel == null)
        {
            playerDataModel = gameObject.GetComponent<PlayerDataModel>();
        }
        if(playerAnimator == null)
        {
            playerAnimator = gameObject.GetComponent<PlayerAnimator>();
        }
        if(rb2d == null)
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
        if(playerUI == null)
        {
            playerUI = gameObject.GetComponent<PlayerUI>();
        }
        if(sfxSource == null)
        {
            sfxSource = GameStats.Instance.SFXSource;
        }
        if (weaponObjects == null)
        {
            weaponObjects = new List<GameObject>();
            weaponObjects.Add(pistolObject);
            weaponObjects.Add(cannonObject);
            weaponObjects.Add(knifeObject);
        }
    }

    public void MovePlayer(float x, float y)
    {
        if (cantMove)
        {
            x = Mathf.Clamp01(Mathf.Abs(x) * 2) * Mathf.Sign(x);
            y = Mathf.Clamp01(Mathf.Abs(y) * 2) * Mathf.Sign(y);
            Vector3 movement = new Vector3(x, y, 0);

            if (Mathf.Abs(x) + Mathf.Abs(y) > playerDataModel.Speedgab)
            {
                rb2d.velocity = movement * playerDataModel.RunSpeed * Time.deltaTime * 50;
            }
            else
            {
                rb2d.velocity = movement * playerDataModel.WalkSpeed * Time.deltaTime * 50;
            }
            playerAnimator.SetBodyDirectionByBlendTree(x, y);
            playerSprite.SetHeadDirectionToBody(x, y);
        }
        else
        {
            playerSprite.SetHeadDirectionToBody(x, y);
        }
    }

    public void RotatePlayer(float x, float y)
    {
        if (x != 0 && y != 0)
        {
            if (playerAnimator.isActiveAndEnabled)
            {
                //playerAnimator.SetAnimationDirection(x, y);
                //playerAnimator.SetBodyAnimationDirection(x, y);
            }
            else
            {
                //playerSprite.SetHeadSpriteToRotation(x, y);
                playerSprite.SetBodySpriteToRotation(x, y);
            }
            
            float heading = Mathf.Atan2(x, y);
            //headPoint.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
            weaponRotationObject.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        }
        MoveCamera(x, y);
    }

    private void MoveCamera(float x, float y)
    {
        CameraPoint.localPosition = new Vector3(x * -1.5f, y * 1.5f);
    }

    public Transform GetCameraPoint()
    {
        return CameraPoint;
    }

    public void SetPlayer(string playerCode, string playerName)
    {
        playerDataModel.SetPlayer(playerCode, playerName);
    }

    public void SetCameraFollow(CameraFollow value)
    {
        playerCamera = value;
    }

    public void ShakeCamera()
    {
        playerCamera.StartCameraShake();
    }

    private void UpdateAttackCooldown()
    {
        if (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        if(activeFireDelay >= 0)
        {
            activeFireDelay -= Time.deltaTime;
        }
    }

    public void ExecuteInteractionButton()
    {
        if (hasPickUp)
        {
            playerDataModel.AddNewWeapon(WeaponCollection.Instance.GetRandomWeapon());
            if(playerDataModel.GetWeaponListCount() == 1)
            {
                attackCooldown = playerDataModel.GetCooldownTime();
            }
            Destroy(pickUpObject);
            pickUpObject = null;
            SetWeaponSprite();
        }
    }

    public void CheckFireWeapon()
    {
        if (attackCooldown <= 0 && playerDataModel.GetWeaponListCount() > 0 && !shootIsInComming )
        {
            if(playerDataModel.GetEquippedWeapon().FireDelay == 0)
            {
                ShootWeapon();
                attackCooldown = playerDataModel.GetCooldownTime();
            }
            else
            {
                shootIsInComming = true;
                sfxSource.PlayOneShot(playerDataModel.GetEquippedWeapon().DelaySound);
                activeFireDelay = playerDataModel.GetEquippedWeapon().FireDelay;
            }  
        }
    }

    private void SetWeaponSprite()
    {
        Instantiate(SmokeEffect, headPoint);
        if (ActiveWeaponObject!= null)
        {
            ActiveWeaponObject.SetActive(false);
            if (!headPoint.gameObject.activeInHierarchy)
            {
                headPoint.gameObject.SetActive(true);
            }
        }
        
        Weapon weapon = playerDataModel.GetEquippedWeapon();
        if(weapon != null)
        {
            foreach(GameObject gobj in weaponObjects)
            {
                gobj.SetActive(false);
            }
            switch (weapon.WeaponName)
            {
                case WeaponCollection.WeaponNames.pistol:
                    SetWeaponSpriteActive(pistolObject, pistolPoint);
                    headPoint.gameObject.SetActive(false);
                    break;
                case WeaponCollection.WeaponNames.cannon:
                    SetWeaponSpriteActive(cannonObject, cannonPoint);
                    headPoint.gameObject.SetActive(false);
                    break;
                case WeaponCollection.WeaponNames.knife:
                    SetWeaponSpriteActive(knifeObject, knifePoint);
                    headPoint.gameObject.SetActive(false);
                    break;
                case WeaponCollection.WeaponNames.snake:
                    SetWeaponSpriteActive(snakeObject, snakePoint);
                    headPoint.gameObject.SetActive(false);
                    break;
                case WeaponCollection.WeaponNames.rocket:
                    SetWeaponSpriteActive(rocketObject, rocketPoint);
                    headPoint.gameObject.SetActive(false);
                    break;
                case WeaponCollection.WeaponNames.laser:
                    SetWeaponSpriteActive(laserObject, laserPoint);
                    headPoint.gameObject.SetActive(false);
                    break;
                case WeaponCollection.WeaponNames.dragon:
                    SetWeaponSpriteActive(dragonObject, dragonPoint);
                    headPoint.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void SetWeaponSpriteActive(GameObject weaponobject, Transform weaponPoint)
    {
        weaponobject.SetActive(true);
        ActiveWeaponObject = weaponobject;
        activeWeaponPoint = weaponPoint;
    }

    public void KillPlayer()
    {
        if (!playerDataModel.IsDead)
        {
            headPoint.gameObject.SetActive(false);
            playerDataModel.KillPlayer();
            if(ActiveWeaponObject != null)
            {
                ActiveWeaponObject.SetActive(false);
            }
            GameController.Instance.PlayerKilled();
            if (playerAnimator.isActiveAndEnabled)
            {
                playerAnimator.SetAnimationToDead();
                playerAnimator.InstantiateDeadEffects(transform);
            }
            else
            {
                playerSprite.SetBodySpriteToDead();
            }
            playerCamera.StartLittleCameraShake();
        }      
    }

    public void ResetPlayer()
    {
        playerSprite.ResetPlayerSprite(); 
        playerAnimator.ResetPlayerAnimation();
        playerAnimator.InstantiateSpawnEffects(transform);
        playerDataModel.ResetPlayer();
        SetWeaponSprite();
        SetHeadState(false);
        StartCoroutine(WaitActivateHead());
        if (playerCamera != null)
        {
            playerCamera.TeleportCamera();
        }
        
    }

    public void AddPoints(int points)
    {
        playerDataModel.AddPoints(points);
        //playerUI.SetPointText(playerDataModel.PlayerScore);
        playerCamera.GetComponent<PlayerCanvasBehaviour>().PopUpScore(points.ToString(),playerDataModel.PlayerScore.ToString());
    }

    public void SetHeadState(bool value)
    {
        headPoint.gameObject.SetActive(value);
    }

    private IEnumerator WaitActivateHead()
    {
        yield return new WaitForSeconds(0.7f);
        SetHeadState(true);
    }

    public void ShootWeapon()
    {
        sfxSource.PlayOneShot(playerDataModel.GetEquippedWeapon().ShootSound);
        BulletFactory.Instance.CreateBullet(activeWeaponPoint,playerDataModel.GetEquippedWeapon().WeaponName, playerDataModel);
        playerDataModel.DeleteWeaponFifo();
        SetWeaponSprite(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PickUp")
        {
            hasPickUp = true;
            pickUpObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PickUp")
        {
            hasPickUp = false;
            pickUpObject = null;
        }
    }

}
