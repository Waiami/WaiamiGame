using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidProjectile : Projectile
{

    public override void DestroyObject()
    {
        GameObject obj = Instantiate(GameStats.Instance.AcidGround, this.gameObject.transform);
        obj.GetComponent<Explosion>().SetReferencePlayerDataModel(base.ReferencePlayerDataModel);
        obj.transform.position = this.transform.position;
        obj.transform.SetParent(null);
        Destroy(this.gameObject);
    }
}
