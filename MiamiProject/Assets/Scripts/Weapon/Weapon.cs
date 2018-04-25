using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private string weaponname;
    [SerializeField] private float equiptime;
    [SerializeField] private float throwawayTime;
    [SerializeField] private float recoil;

    public string WeaponName { get { return weaponname; } }
    public float EqiupTime { get { return equiptime; } }
    public float ThrowAwayTime { get { return throwawayTime; } }
    public float Recoil { get { return recoil; } }
}
