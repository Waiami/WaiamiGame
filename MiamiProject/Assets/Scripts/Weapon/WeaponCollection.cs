using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollection : MonoBehaviour {

    [SerializeField] private List<Weapon> Weapons;
    
    public enum WeaponNames {empty, pistol, knife, cannon};

    public Weapon GetRandomWeapon()
    {
        if(GameStats.Instance.OnlySpawnWeapons == WeaponNames.empty)
        {
            return Weapons[Random.Range(0, Weapons.Count)];
        }
        else
        {
            foreach(Weapon w in Weapons)
            {
                if(w.WeaponName == GameStats.Instance.OnlySpawnWeapons)
                {
                    return w;
                }
            }
            return Weapons[Random.Range(0, Weapons.Count)];
        }
        
    }

    #region Instance
    public static WeaponCollection Instance;

    void Awake()
    {
        if (Instance != null)
        {
            if (this != Instance)
            {
                Destroy(this);
            }
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }


    }
    #endregion
}
