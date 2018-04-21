using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory : MonoBehaviour {


    #region Prefabs
    [SerializeField] public GameObject FiveMMBullet;
    #endregion

    #region public Methods
    public void CreatePullet(Transform spawnTransform, string weaponname, string playerCode)
    {
        switch (weaponname)
        {
            case "pistol":

            default:
                break;
        }
    }
    #endregion

    #region private Methods

    private void SpawnFiveMMBullet(Transform spawnTransform, string PlayerCode)
    {
        GameObject obj = Instantiate(FiveMMBullet, spawnTransform);
        obj.transform.tag = "Bullet_" + PlayerCode;
        obj.transform.SetParent(null); 
    }

    #endregion


    #region Instance
    public static WeaponFactory Instance;

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
