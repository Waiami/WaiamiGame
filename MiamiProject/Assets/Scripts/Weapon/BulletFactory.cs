using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour {


    #region Prefabs
    [SerializeField] public GameObject FiveMMBullet;
    [SerializeField] public GameObject CannonBall;
    [SerializeField] public GameObject KnifeSlash;
    #endregion

    #region public Methods
    public void CreateBullet(Transform spawnTransform, WeaponCollection.WeaponNames weaponname, PlayerDataModel playerDataModel)
    {
        
        switch (weaponname)
        {
            case WeaponCollection.WeaponNames.knife:
                SpawnKnifeSlash(spawnTransform, playerDataModel);
                break;
            case WeaponCollection.WeaponNames.cannon:
                SpawnCannonBall(spawnTransform, playerDataModel);
                break;
            case WeaponCollection.WeaponNames.pistol:
                SpawnFiveMMBullet(spawnTransform, playerDataModel);
                break;
            default:
                break;
        }

        
    }
    #endregion

    #region private Methods

    private void SpawnFiveMMBullet(Transform spawnTransform, PlayerDataModel playerDataModel)
    {
        GameObject obj = Instantiate(FiveMMBullet, spawnTransform);
        obj.GetComponent<Projectile>().SetPlayerDataModel(playerDataModel);
        obj.transform.SetParent(null); 
    }

    private void SpawnCannonBall(Transform spawnTransform, PlayerDataModel playerDataModel)
    {
        GameObject obj = Instantiate(CannonBall, spawnTransform);
        obj.GetComponent<ExplosiveProjectile>().SetPlayerDataModel(playerDataModel);
        obj.transform.SetParent(null);
    }

    public void SpawnKnifeSlash(Transform spawnTransform, PlayerDataModel playerDataModel)
    {
        GameObject obj = Instantiate(KnifeSlash, spawnTransform);
        obj.GetComponent<KnifeSlash>().SetPlayerDataModel(playerDataModel);
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
