using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    private List<string> weaponList;
    private int lives;
    private bool equipped;
    private string equippedWeapon;

	// Use this for initialization
	void Start () {
        equipped = false;
        lives = 4;
        weaponList = new List<string> { "suckerPunsh", "pistol","uzi", "knife","shotgun","sword" };
        equippedWeapon = weaponList[1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void decreaseLives()
    {
        lives--;
    }


    public string GetWeapon()
    {
        return equippedWeapon;
    }


}
