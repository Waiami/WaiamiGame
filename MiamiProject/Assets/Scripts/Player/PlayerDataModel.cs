using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel : MonoBehaviour {

    [SerializeField] private string playerCode = "P1";
    [SerializeField] private float walkspeed = 5;
    [SerializeField] private float runspeed = 7;
    [SerializeField] private float speedgab = 0.7f;
    [SerializeField] private float controllerthreshhold = 0.3f;

    [SerializeField] private PlayerController playerController;
    private Queue<Weapon> playerWeaponList;
    private Weapon equippedWeapon;
    private bool isDead;

    public bool IsDead { get { return isDead; } }
    public float WalkSpeed { get { return walkspeed; } }
    public float RunSpeed { get { return runspeed; } }
    public float Speedgab { get { return speedgab; } }
    public float ControllerThreshhold { get { return controllerthreshhold; } }
    public string PlayerCode { get { return playerCode; } }

    // Use this for initialization
    void Start () {
        isDead = false; 
        playerWeaponList = new Queue<Weapon>();
        if(playerWeaponList.Count > 0)
        {
            Weapon w = playerWeaponList.Peek();
        }
        Inisialize();

    }

    void Inisialize()
    {
        walkspeed = GameStats.Instance.WalkSpeed;
        runspeed = GameStats.Instance.RunSpeed;
        speedgab = GameStats.Instance.Speedgab;
        controllerthreshhold = GameStats.Instance.Threshhold;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public string GetEquippedWeapon()
    {
        if(equippedWeapon== null)
        {
            return "";
        }
        return equippedWeapon.name.ToLower();
    }

    public void KillPlayer()
    {
        playerController.SetPlayerToDead();
        isDead = true;
    }

    public void AddNewWeapon(Weapon weapon)
    {
        playerWeaponList.Enqueue(weapon);
        equippedWeapon = weapon;
        DebugLogWeaponList();
    }

    public void DeleteWeaponFifo()
    {
        equippedWeapon = null;
        if(playerWeaponList.Count > 0)
        {
            equippedWeapon = playerWeaponList.Dequeue();
            if(playerWeaponList.Count < 1)
            {
                equippedWeapon = null;
            }
            DebugLogWeaponList();
        }
        
    }

    private void DebugLogWeaponList()
    {
        if(playerWeaponList.Count > 0)
        {
            string returnstring = "";
            foreach (Weapon s in playerWeaponList)
            {
                returnstring += s.name + ", ";
            }
            Debug.Log(returnstring);
        }
        else
        {
            Debug.Log("No Weapons left");
        }
        
    }


}
