using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    private List<string> weaponList;
    private bool equipped;
    private string equippedWeapon;
    private bool dead;

    public bool Dead { get { return dead; } set { dead = value; } }

    // Use this for initialization
    void Start () {
        dead = false; 
        equipped = false;
        weaponList = new List<string> { "suckerPunsh", "pistol","uzi", "knife","shotgun","sword" };
        equippedWeapon = weaponList[1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public string GetWeapon()
    {
        return equippedWeapon;
    }

    public void KillPlayer()
    {

    }


}
