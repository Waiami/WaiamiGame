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
    [SerializeField] private Transform CameraPoint;
    [SerializeField] private CameraFollow playerCamera;

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

    private bool shootIsInComming = false;
    private float activeFireDelay;
    [SerializeField]private bool cantMove;
    public bool CantMove { set { cantMove = value; } }

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
        boolSuckerPunsh = false;
        suckerPunsh = GameStats.Instance.SuckerPunsh;
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
            playerAnimator.SetBodyAnimationDirection(x, y);
            playerAnimator.SetBlendFloat(Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)));
        }
        else
        {
            playerAnimator.SetBlendFloat(0);
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
            switch (weapon.WeaponName)
            {
                case WeaponCollection.WeaponNames.pistol:
                    SetWeaponSpriteActive(pistolObject, pistolPoint);
                    break;
                case WeaponCollection.WeaponNames.cannon:
                    SetWeaponSpriteActive(cannonObject, cannonPoint);
                    break;
                case WeaponCollection.WeaponNames.knife:
                    SetWeaponSpriteActive(knifeObject, knifePoint);
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
            playerCamera.StartLittleCameraShake();
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

    //Not in Use anymore
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
