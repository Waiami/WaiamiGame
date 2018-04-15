using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    [SerializeField] private GameData gameData;
    private List<string> weaponList;

    private void Start()
    {
        if(gameData == null)
        {
            gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        }
        weaponList = gameData.WeaponList;
    }

    public string GetRandomWeapon()
    {
        return weaponList[Random.Range(0, weaponList.Count)]; 
    }
}
