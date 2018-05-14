using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    #region variables
    [SerializeField] private WeaponCollection.WeaponNames weaponname;
    [SerializeField] private float equiptime;
    [SerializeField] private float throwawayTime;
    [SerializeField] private float recoil;
    [SerializeField] private float fireDelay;
    #endregion

    #region setter
    public WeaponCollection.WeaponNames WeaponName { get { return weaponname; }}
    public float EqiupTime { get { return equiptime; } }
    public float ThrowAwayTime { get { return throwawayTime; } }
    public float Recoil { get { return recoil; } }
    public float FireDelay { get { return fireDelay; } }
#endregion
}
