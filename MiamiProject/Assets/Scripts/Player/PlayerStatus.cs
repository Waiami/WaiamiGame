using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameData gameData;
    private Queue<string> playerWeaponList;
    private string equippedWeapon;
    private bool isDead;

    public bool IsDead { get { return isDead; } }

    // Use this for initialization
    void Start () {
        isDead = false; 
        playerWeaponList = new Queue<string>();
        if(playerWeaponList.Count > 0)
        {
            equippedWeapon = playerWeaponList.Peek();
        }
        
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
