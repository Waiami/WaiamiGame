using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{

    public override void DestroyObject()
    {
        GameObject obj = Instantiate(GameStats.Instance.Explosion, this.gameObject.transform);
        obj.GetComponent<Explosion>().SetReferencePlayerDataModel(base.ReferencePlayerDataModel);
        obj.transform.position = this.transform.position;
        obj.transform.SetParent(null);
        Destroy(this.gameObject);
    }

}
