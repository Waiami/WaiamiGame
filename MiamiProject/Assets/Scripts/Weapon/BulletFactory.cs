﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour {


    #region Prefabs
    [SerializeField] public GameObject FiveMMBullet;
    #endregion

    #region public Methods
    public void CreateBullet(Transform spawnTransform, string weaponname, string playerCode)
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
    public static BulletFactory Instance;

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