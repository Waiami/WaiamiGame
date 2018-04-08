using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private string playerCode = "P1";
    [SerializeField]
    private GameObject PlayerSpriteObject;
    [SerializeField]
    private PlayerStatus playerStatus;
    private string inputHorizontal = "Horizontal_";
    private string inputVertical = "Vertical_";
    private string inputRotateHorizontal = "RotateHorizontal_";
    private string inputRotateVertical = "RotateVertical_";
    private string aButton = "AButton_";
    private string fireButton = "Fire_";

    private Rigidbody2D rb2d;

    [Header("SuckerPunsh")]
    [SerializeField]
    private float punshDelay = 1f;
    private float punshCooldown = -1f;
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject rightHand;
    private bool boolSuckerPunsh;
    private GameObject suckerPunsh;
    private GameObject pistolBullet;

    [Header("GeneralWeapons")]
    [SerializeField]
    private float attackDelay = 0.3f;
    private float attackCooldown = -1f;

    [Header("Pistol")]
    [SerializeField]
    private Transform pistolPoint;

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
        suckerPunsh = GameData.Instance.SuckerPunsh;
        pistolBullet = GameData.Instance.FivemmBullet;
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
        PressFire();
    }

    private void MovePlayer()
    {
        var x = Input.GetAxis(inputHorizontal);
        var y = Input.GetAxis(inputVertical);
        Vector3 movement = new Vector3(x, y, 0);
        rb2d.velocity = movement * speed;
    }

    private void RotatePlayer()
    {
        float rotateHorizontal = Input.GetAxis(inputRotateHorizontal);
        float rotateVertical = Input.GetAxis(inputRotateVertical);
        if (rotateHorizontal != 0 && rotateVertical != 0)
        {
            float heading = Mathf.Atan2(rotateHorizontal, rotateVertical);

            PlayerSpriteObject.transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        }
    }


    private void PressInteration()
    {
        if (Input.GetButton(aButton))
        {

        }
    }

    private void PressFire()
    {
        var x = Input.GetAxis(fireButton);

        if(x > 0.7f)
        {
            Fire();
        }
    }

    private void Fire()
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
            
            GameObject projectile = Instantiate(pistolBullet, pistolPoint);
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
        projectile.GetComponent<Projectile>().SetPlayerCode(playerCode);
        projectile.transform.SetParent(null);
    }

   
}
