using UnityEngine;

public class PlayerController : MonoBehaviour {

#region variables
    
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private PlayerDataModel playerDataModel;
    [SerializeField] private PlayerSprite playerSprite;
    [SerializeField] private PlayerAnimator playerAnimator;

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

    [Header("Pistol")]
    [SerializeField]private Transform pistolPoint;
    [SerializeField] private GameObject pistolObject;


    private bool hasPickUp = false;
    private GameObject pickUpObject;
    #endregion

    // Use this for initialization
    void Start () {
        Initialize();
	}

    private void FixedUpdate()
    {
        UpdateAttackCooldown();
    }

    #region initialize
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
    }
    #endregion

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
        float thresholded = 0.2f;
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
            if (Mathf.Abs(x) > thresholded || Mathf.Abs(y) > thresholded)
            {
                float heading = Mathf.Atan2(x, y);
                weaponRotationObject.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
            }
        }
    }

    private void UpdateAttackCooldown()
    {
        if (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
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
        if(attackCooldown <= 0)
        {
            WeaponCollection.WeaponNames weapon = playerDataModel.GetEquippedWeapon();
            if (weapon == WeaponCollection.WeaponNames.knife || weapon == WeaponCollection.WeaponNames.pistol || weapon == WeaponCollection.WeaponNames.shotgun)
            {
                PistolShot();
                attackCooldown =  playerDataModel.GetCooldownTime();
            }
        }
    }

    private void SetWeaponSprite()
    {
        if(ActiveWeaponObject!= null)
        {
            ActiveWeaponObject.SetActive(false);
        }

        WeaponCollection.WeaponNames weapon = playerDataModel.GetEquippedWeapon();
        if(weapon != WeaponCollection.WeaponNames.empty)
        {
            if (weapon == WeaponCollection.WeaponNames.knife || weapon == WeaponCollection.WeaponNames.pistol || weapon == WeaponCollection.WeaponNames.shotgun)
            {
                pistolObject.SetActive(true);
                ActiveWeaponObject = pistolObject;
            }
        }
        
    }

    public void SetPlayerToDead()
    {
        if (playerAnimator.isActiveAndEnabled)
        {
            playerAnimator.SetAnimationToDead();
        }
        else
        {
            playerSprite.SetBodySpriteToDead();
        }
        
    }

    public void ResetPlayer()
    {
        playerSprite.ResetPlayerSprite(); 
        playerAnimator.ResetPlayerAnimation();
        playerDataModel.ResetPlayer();
        SetWeaponSprite();
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

    public void PistolShot()
    {
        BulletFactory.Instance.CreateBullet(pistolPoint,playerDataModel.GetEquippedWeapon(), playerDataModel.PlayerCode);
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
