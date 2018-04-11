using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]   private float walkspeed = 5;
    [SerializeField]   private float runspeed = 7;
    [SerializeField]   private float speedgab = 0.7f;
    [SerializeField]   private string playerCode = "P1";
    public string PlayerCode { get { return playerCode; } }
    [SerializeField]   private GameObject PlayerSpriteObject;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private PlayerStatus playerStatus;
    private string inputHorizontal = "Horizontal_";
    private string inputVertical = "Vertical_";
    private string inputRotateHorizontal = "RotateHorizontal_";
    private string inputRotateVertical = "RotateVertical_";
    private string aButton = "AButton_";
    private string fireButton = "Fire_";
    private bool isDead;

    private Rigidbody2D rb2d;

    [Header("SuckerPunsh")]
    [SerializeField]
    private float punshDelay = 1f;
    private float punshCooldown = -1f;
    [SerializeField]private GameObject leftHand;
    [SerializeField]private GameObject rightHand;
    private bool boolSuckerPunsh;
    private GameObject suckerPunsh;
    private GameObject pistolBullet;

    [Header("GeneralWeapons")]
    [SerializeField]private float attackDelay = 0.3f;
    private float attackCooldown = -1f;
    [SerializeField]private float weaponradius = 0.3f;

    [Header("Pistol")]
    [SerializeField]private Transform pistolPoint;
    [SerializeField]private GameObject weaponObject;

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

    // Use this for initialization
    void Start () {
        Initialize();
	}

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
        isDead = false;
}

    void FixedUpdate()
    {
        if (!isDead)
        {
            MovePlayer();
            RotatePlayer();
        }
           
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckIfFireButtonPressed();
        }
        
    }

    private void MovePlayer()
    {
        var x = Input.GetAxis(inputHorizontal);
        var y = Input.GetAxis(inputVertical);
        if(x < 0.3 && x > -0.3 ){ x = 0;}
        if(y < 0.3 && y > -0.3){y = 0;}
        Vector3 movement = new Vector3(x, y, 0);
        if(Mathf.Abs(x) > speedgab || Mathf.Abs(y) > speedgab)
        {
            rb2d.velocity = movement * runspeed;
        }
        else
        {
            rb2d.velocity = movement * walkspeed;
        }
        if(x == 0 && y == 0){
            charAnimator.SetTrigger("Stop");
        }
        else
        {
            if(charDirection == "up")
            {
                charAnimator.SetTrigger("MoveUp");
            }
            if (charDirection == "down")
            {
                charAnimator.SetTrigger("MoveDown");
            }
            if (charDirection == "left")
            {
                charAnimator.SetTrigger("MoveLeft");
            }
            if (charDirection == "right")
            {
                charAnimator.SetTrigger("MoveRight");
            }
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
                charDirection = "up";
            }
            else if (y < -thresholded && Mathf.Abs(y) > Mathf.Abs(x))
            {
                playerSpriteRenderer.sprite = CharDown;
                charDirection = "down";
            }
            else if (x > thresholded && x > Mathf.Abs(y))
            {
                playerSpriteRenderer.sprite = CharLeft;
                charDirection = "left";
            }
            else if (x < -thresholded && Mathf.Abs(x) > Mathf.Abs(y))
            {
                playerSpriteRenderer.sprite = CharRight;
                charDirection = "right";
            }

            if(Mathf.Abs(x) > thresholded || Mathf.Abs(y) > thresholded)
            {
                float heading = Mathf.Atan2(x, y);
                weaponObject.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
            }
            
        }
    }


    private void PressInteraktionButton()
    {
        if (Input.GetButton(aButton))
        {

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
        string weapon = playerStatus.GetWeapon();
        if(weapon == "suckerPunsh")
        {
            SuckerPunsh();
        }
        if(weapon == "pistol")
        {
            PistolShot();
        }
    }
    

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

    public void KillPlayer()
    {
        isDead = true;
        playerSpriteRenderer.sprite = CharDead;
    }


}
