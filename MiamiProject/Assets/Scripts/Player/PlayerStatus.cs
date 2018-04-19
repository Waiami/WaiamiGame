using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {


    [SerializeField] private float walkspeed = 5;
    [SerializeField] private float runspeed = 7;
    [SerializeField] private float speedgab = 0.7f;
    [SerializeField] private float threshhold = 0.3f;

    [SerializeField] private PlayerController playerController;
    [SerializeField]
    private GameData gameData;
    private Queue<string> playerWeaponList;
    private string equippedWeapon;
    private bool isDead;

    public bool IsDead { get { return isDead; } }
    public float WalkSpeed { get { return walkspeed; } }
    public float RunSpeed { get { return runspeed; } }
    public float Speedgab { get { return speedgab; } }
    public float Threshhold { get { return threshhold; } }

    // Use this for initialization
    void Start () {
        isDead = false; 
        playerWeaponList = new Queue<string>();
        if(playerWeaponList.Count > 0)
        {
            equippedWeapon = playerWeaponList.Peek();
        }
        Inisialize();

    }

    void Inisialize()
    {
        walkspeed = GameData.Instance.WalkSpeed;
        runspeed = GameData.Instance.RunSpeed;
        speedgab = GameData.Instance.Speedgab;
        threshhold = GameData.Instance.Threshhold;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public string GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    public void KillPlayer()
    {
        playerController.SetSpriteToDead();
        isDead = true;
    }

    public void AddNewWeapon(string weapon)
    {
        playerWeaponList.Enqueue(weapon);
        equippedWeapon = weapon;
        DebugLogWeaponList();
    }

    public void DeleteWeaponFifo()
    {
        equippedWeapon = "";
        if(playerWeaponList.Count > 0)
        {
            equippedWeapon = playerWeaponList.Dequeue();
            if(playerWeaponList.Count < 1)
            {
                equippedWeapon = "";
            }
            DebugLogWeaponList();
        }
        
    }

    private void DebugLogWeaponList()
    {
        if(playerWeaponList.Count > 0)
        {
            string returnstring = "";
            foreach (string s in playerWeaponList)
            {
                returnstring += s + ", ";
            }
            Debug.Log(returnstring);
        }
        else
        {
            Debug.Log("No Weapons left");
        }
        
    }


}
