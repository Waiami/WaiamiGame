using UnityEngine;

public class PlayerController : MonoBehaviour {

#region variables
    [SerializeField] private float walkspeed = 5;
    [SerializeField] private float runspeed = 7;
    [SerializeField] private float speedgab = 0.7f;
    [SerializeField] private float threshhold = 0.3f;
    [SerializeField] private string playerCode = "P1";
    public string PlayerCode { get { return playerCode; } }
    [SerializeField] private GameObject PlayerSpriteObject;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private PlayerStatus playerStatus;
    private string inputHorizontal = "Horizontal_";
    private string inputVertical = "Vertical_";
    private string inputRotateHorizontal = "RotateHorizontal_";
    private string inputRotateVertical = "RotateVertical_";
    private string aButton = "A_";
    private string fireButton = "Fire_";

    private Rigidbody2D rb2d;

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
    private float attackCooldown = -1f;
    private GameObject ActiveWeaponObject;

    [Header("Pistol")]
    [SerializeField]private Transform pistolPoint;
    [SerializeField] private GameObject pistolObject;
    

    [Header("Sprites")]
    [SerializeField]private Sprite CharUp;
    [SerializeField]private Sprite CharDown;
    [SerializeField]private Sprite CharLeft;
    [SerializeField]private Sprite CharRight;
    [SerializeField] private Sprite CharDead;

    [Header("Animator")]
    [SerializeField]
    private Animator charAnimator;
    private string charDirection = "up";


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
        inputHorizontal = inputHorizontal + playerCode;
        inputVertical = inputVertical + playerCode;
        inputRotateHorizontal = inputRotateHorizontal + playerCode;
        inputRotateVertical = inputRotateVertical + playerCode;
        aButton = aButton + playerCode;
        fireButton = fireButton + playerCode;
        boolSuckerPunsh = false;
        rb2d = PlayerSpriteObject.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = PlayerSpriteObject.GetComponent<SpriteRenderer>();
        suckerPunsh = GameData.Instance.SuckerPunsh;
        pistolBullet = GameData.Instance.FivemmBullet;

        walkspeed = GameData.Instance.WalkSpeed;
        runspeed = GameData.Instance.RunSpeed;
        attackDelay = GameData.Instance.AttackDelay;
    }
#endregion

    void FixedUpdate()
    {
        if (!playerStatus.IsDead)
        {
            MovePlayer();
            RotatePlayer();
        }
           
    }

    private void Update()
    {
        if (!playerStatus.IsDead)
        {
            CheckIfFireButtonPressed();
            CheckIfInteraktionButtonPressed();
        }
        
    }

    private void MovePlayer()
    {
        var x = Input.GetAxis(inputHorizontal);
        var y = Input.GetAxis(inputVertical);
        if(Mathf.Abs(x) < threshhold ){ x = 0;}
        if (Mathf.Abs(y) < threshhold){y = 0;}
        Vector3 movement = new Vector3(x, y, 0);
        if(Mathf.Abs(x) > speedgab || Mathf.Abs(y) > speedgab)
        {
            rb2d.velocity = movement * runspeed;
        }
        else
        {
            rb2d.velocity = movement * walkspeed;
            
        }
        if(x == 0 && y == 0)
        {
            charAnimator.SetFloat("Blend", 0);
        }
        else
        {
            charAnimator.SetFloat("Blend", Mathf.Max(Mathf.Abs(x),Mathf.Abs(y)));
            
        }
        
    }

    private void RotatePlayer()
    {
        float x = Input.GetAxis(inputRotateHorizontal);
        float y = Input.GetAxis(inputRotateVertical);
        float thresholded = 0.2f;
        if (x != 0 && y != 0)
        {
            if (y > thresholded &&  y > (Mathf.Abs(x)))
            {
                playerSpriteRenderer.sprite = CharUp;
                charAnimator.SetTrigger("MoveUp");
            }
            else if (y < -thresholded && Mathf.Abs(y) > Mathf.Abs(x))
            {
                playerSpriteRenderer.sprite = CharDown;
                charAnimator.SetTrigger("MoveDown");
            }
            else if (x > thresholded && x > Mathf.Abs(y))
            {
                playerSpriteRenderer.sprite = CharLeft;
                charAnimator.SetTrigger("MoveLeft");
            }
            else if (x < -thresholded && Mathf.Abs(x) > Mathf.Abs(y))
            {
                playerSpriteRenderer.sprite = CharRight;
                charAnimator.SetTrigger("MoveRight");
            }

            if(Mathf.Abs(x) > thresholded || Mathf.Abs(y) > thresholded)
            {
                float heading = Mathf.Atan2(x, y);
                weaponRotationObject.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
            }
            
        }
    }


    private void CheckIfInteraktionButtonPressed()
    {
        if (Input.GetButton(aButton))
        {
            if (hasPickUp)
            {
                playerStatus.AddNewWeapon(pickUpObject.GetComponent<PickUp>().GetRandomWeapon());
                Destroy(pickUpObject);
                pickUpObject = null;
                SetWeaponSprite();
            }
        }
    }

    private void CheckIfFireButtonPressed()
    {
        var x = Input.GetAxis(fireButton);

        if(x > 0.7f)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        string weapon = playerStatus.GetEquippedWeapon();
        if(weapon == "suckerPunsh")
        {
            SuckerPunsh();
        }
        if(weapon == "pistol" || weapon == "knife" || weapon == "shotgun")
        {
            PistolShot();
            
        }
    }

    private void SetWeaponSprite()
    {
        if(ActiveWeaponObject!= null)
        {
            ActiveWeaponObject.SetActive(false);
        }
        
        string weapon = playerStatus.GetEquippedWeapon();
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
        playerSpriteRenderer.sprite = CharDead;
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
        if (attackCooldown < 0)
        {
            GameObject projectile = GameObject.Instantiate(GameData.Instance.FivemmBullet, pistolPoint);
            PrepareProjectile(projectile);
            playerStatus.DeleteWeaponFifo();
            SetWeaponSprite();
            attackCooldown = attackDelay;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void PrepareProjectile(GameObject projectile)
    {
        projectile.tag = "Bullet_" + playerCode;
        projectile.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PickUp")
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

    #endregion



}
