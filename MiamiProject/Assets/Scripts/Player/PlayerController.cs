using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

#region variables
    
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private PlayerDataModel playerDataModel;
    [SerializeField] private PlayerSprite playerSprite;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private PlayerUI playerUI;

    public PlayerDataModel PlayerModelScript { get { return playerDataModel; } }

    private float attackCooldown = -1;

    [Header("SuckerPunsh")]
    [SerializeField]
    private float punshDelay = 1f;
    private float punshCooldown = -1f;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    private bool boolSuckerPunsh;
    private GameObject suckerPunsh;

    [Header("GeneralWeapons")]
    [SerializeField]private float attackDelay = 0.3f;
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


    private bool hasPickUp = false;
    private GameObject pickUpObject;

    private bool cannonballInComming = false;
    private float activeFireDelay;

    private AudioSource sfxSource;
    #endregion

    // Use this for initialization
    void Start () {
        Initialize();
        playerDataModel.AddPoints(GameStats.Instance.PlayerStartScore);
        playerUI.SetPointText(playerDataModel.PlayerScore);
	}

    private void FixedUpdate()
    {
        UpdateAttackCooldown();
        if (cannonballInComming)
        {
            if (activeFireDelay < 0)
            {
                cannonballInComming = false;
                ShootWeapon();
                sfxSource.PlayOneShot(GameStats.Instance.CanonSound);
            }
        }
    }

    void Initialize()
    {
        boolSuckerPunsh = false;
        suckerPunsh = GameStats.Instance.SuckerPunsh;
        attackDelay = 0;
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
        if(Mathf.Abs(x) < playerDataModel.ControllerThreshhold ) {x = 0;}
        if (Mathf.Abs(y) < playerDataModel.ControllerThreshhold) {y = 0;}
        Vector3 movement = new Vector3(x, y, 0);
        if (Mathf.Abs(x) + Mathf.Abs(y) > playerDataModel.Speedgab )
        {
            rb2d.velocity = movement * playerDataModel.RunSpeed * Time.deltaTime * 50;
        }
        else
        {
            rb2d.velocity = movement * playerDataModel.WalkSpeed * Time.deltaTime * 50;

        }
        playerAnimator.SetBlendFloat(Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)));
    }

    public void RotatePlayer(float x, float y)
    {
        float thresholded = 0.05f;
        if (x != 0 && y != 0)
        {
            if (playerAnimator.isActiveAndEnabled)
            {
                playerAnimator.SetAnimationDirection(x, y);
            }
            else
            {
                playerSprite.SetBodySpriteToRotation(x, y);
            }
            float heading = Mathf.Atan2(x, y);
            weaponRotationObject.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
            if (Mathf.Abs(x) > thresholded || Mathf.Abs(y) > thresholded)
            {
                
            }
        }
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

    public void FireWeapon()
    {
        if (attackCooldown <= 0 && playerDataModel.GetWeaponListCount() > 0 && !cannonballInComming )
        {        
            switch (playerDataModel.GetEquippedWeapon().WeaponName)
            {
                case WeaponCollection.WeaponNames.knife:
                    ShootWeapon();
                    attackCooldown = playerDataModel.GetCooldownTime();
                    break;
                case WeaponCollection.WeaponNames.pistol:
                    ShootWeapon();
                    attackCooldown = playerDataModel.GetCooldownTime();
                    sfxSource.PlayOneShot(GameStats.Instance.PistolSound);
                    break;
                case WeaponCollection.WeaponNames.cannon:
                    cannonballInComming = true;
                    activeFireDelay = playerDataModel.GetEquippedWeapon().FireDelay;
                    sfxSource.PlayOneShot(GameStats.Instance.SlowmatchSound);
                    break;

            }
        }
    }

    private void SetWeaponSprite()
    {
        if(ActiveWeaponObject!= null)
        {
            ActiveWeaponObject.SetActive(false);
        }

        Weapon weapon = playerDataModel.GetEquippedWeapon();
        if(weapon != null)
        {
            foreach(GameObject gobj in weaponObjects)
            {
                gobj.SetActive(false);
            }
            if (weapon.WeaponName == WeaponCollection.WeaponNames.pistol)
            {
                pistolObject.SetActive(true);
                ActiveWeaponObject = pistolObject;
                activeWeaponPoint = pistolPoint;
            }
            if(weapon.WeaponName == WeaponCollection.WeaponNames.cannon)
            {
                cannonObject.SetActive(true);
                ActiveWeaponObject = cannonObject;
                activeWeaponPoint = cannonPoint;
            }
            if (weapon.WeaponName == WeaponCollection.WeaponNames.knife)
            {
                knifeObject.SetActive(true);
                ActiveWeaponObject = knifeObject;
                activeWeaponPoint = knifePoint;
            }
        }
        
    }

    public void KillPlayer()
    {
        if (!playerDataModel.IsDead)
        {
            playerDataModel.KillPlayer();
            GameController.Instance.PlayerKilled();
            if (playerAnimator.isActiveAndEnabled)
            {
                playerAnimator.SetAnimationToDead();
            }
            else
            {
                playerSprite.SetBodySpriteToDead();
            }
        }      
    }

    public void ResetPlayer()
    {
        playerSprite.ResetPlayerSprite(); 
        playerAnimator.ResetPlayerAnimation();
        playerDataModel.ResetPlayer();
        SetWeaponSprite();
    }

    public void AddPoints(int points)
    {
        playerDataModel.AddPoints(points);
        playerUI.SetPointText(playerDataModel.PlayerScore);
    }

    #region weapons

    private void SuckerPunsh()
    {
        if(punshCooldown < 0)
        {
            
            if (boolSuckerPunsh)
            {
                boolSuckerPunsh = false;
                GameObject projectile = Instantiate(suckerPunsh, leftHand.transform);
                projectile.transform.SetParent(null);
            }
            else
            {
                boolSuckerPunsh = true;
                GameObject projectile = Instantiate(suckerPunsh, rightHand.transform);
                projectile.transform.SetParent(null);
            }
            punshCooldown = punshDelay;
        }
        else
        {
            punshCooldown -= Time.deltaTime;
        }
    }

    public void ShootWeapon()
    {
        BulletFactory.Instance.CreateBullet(activeWeaponPoint,playerDataModel.GetEquippedWeapon().WeaponName, playerDataModel);
        playerDataModel.DeleteWeaponFifo();
        SetWeaponSprite(); 
    }

    #endregion

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
