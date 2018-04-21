using UnityEngine;

public class PlayerController : MonoBehaviour {

#region variables
    
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerSprite playerSprite;
    [SerializeField] private PlayerAnimator playerAnimator;

    public PlayerModel PlayerModelScript { get { return playerModel; } }

    

    [Header("SuckerPunsh")]
    [SerializeField]
    private float punshDelay = 1f;
    private float punshCooldown = -1f;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    private bool boolSuckerPunsh;
    private GameObject suckerPunsh;
    private GameObject pistolBullet;

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

#region initialize
    void Initialize()
    {
        boolSuckerPunsh = false;
        suckerPunsh = GameStats.Instance.SuckerPunsh;
        pistolBullet = GameStats.Instance.FivemmBullet;
        attackDelay = GameStats.Instance.AttackDelay;
        if(playerSprite == null)
        {
            playerSprite = gameObject.GetComponent<PlayerSprite>();
        }
        if(playerModel == null)
        {
            playerModel = gameObject.GetComponent<PlayerModel>();
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
        if(Mathf.Abs(x) < playerModel.ControllerThreshhold ) {x = 0;}
        if (Mathf.Abs(y) < playerModel.ControllerThreshhold) {y = 0;}
        Vector3 movement = new Vector3(x, y, 0);
        if (Mathf.Abs(x) > playerModel.Speedgab || Mathf.Abs(y) > playerModel.Speedgab)
        {
            rb2d.velocity = movement * playerModel.RunSpeed;
        }
        else
        {
            rb2d.velocity = movement * playerModel.WalkSpeed;

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


    public void ExecuteInteractionButton()
    {
        if (hasPickUp)
        {
            playerModel.AddNewWeapon(pickUpObject.GetComponent<PickUp>().GetRandomWeapon());
            Destroy(pickUpObject);
            pickUpObject = null;
            SetWeaponSprite();
        }
    }

    public void FireWeapon()
    {
        string weapon = playerModel.GetEquippedWeapon();
        if(weapon == "suckerPunsh")
        {
            SuckerPunsh();
        }
        if(weapon == "pistol" || weapon == "knife" || weapon == "shotgun")
        {
            PistolShot();
            
        }
    }

    public float GetNewAttackCooldown()
    {
        return attackDelay;
    }

    private void SetWeaponSprite()
    {
        if(ActiveWeaponObject!= null)
        {
            ActiveWeaponObject.SetActive(false);
        }
        
        string weapon = playerModel.GetEquippedWeapon();
        if(weapon != "")
        {
            if (weapon == "pistol" || weapon == "knife" || weapon == "shotgun")
            {
                pistolObject.SetActive(true);
                ActiveWeaponObject = pistolObject;
            }
        }
        
    }

    public void SetSpriteToDead()
    {
        playerSprite.SetBodySpriteToDead();
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
        GameObject projectile = GameObject.Instantiate(GameStats.Instance.FivemmBullet, pistolPoint);
        PrepareProjectile(projectile);
        playerModel.DeleteWeaponFifo();
        SetWeaponSprite();
    }

    private void PrepareProjectile(GameObject projectile)
    {
        projectile.tag = "Bullet_" + playerModel.PlayerCode;
        projectile.transform.SetParent(null);
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
