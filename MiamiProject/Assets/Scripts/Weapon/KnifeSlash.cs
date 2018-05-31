using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSlash : MonoBehaviour {

    [SerializeField] private Animator slashAnimator;
    private PlayerDataModel referencePlayerDataModel;
    private bool noDamage;
    
    public PlayerDataModel ReferencePlayerDataModel { get { return referencePlayerDataModel; } }
    void Start()
    {
        if (slashAnimator == null)
        {
            slashAnimator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (slashAnimator.GetCurrentAnimatorStateInfo(0).IsName("Any"))
        {
            this.transform.SetParent(null);
            Destroy(this.gameObject);
        }
    }

    public void SetPlayerDataModel(PlayerDataModel value)
    {
        referencePlayerDataModel = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "Wall")
        {
            noDamage = true;
        }
        if (collision.tag == "Player")
        {
            if (referencePlayerDataModel != collision.gameObject.GetComponent<PlayerDataModel>())
            {
                if (!noDamage)
                {
                    referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(collision.gameObject.GetComponent<PlayerDataModel>().PointsWorth);
                    collision.gameObject.GetComponent<PlayerController>().KillPlayer();
                }
            }
        }
        if (collision.GetComponent<NPC>() != null)
        {
            if (!noDamage && collision.GetComponent<NPC>().Dead == false)
            {
                referencePlayerDataModel.GetComponent<PlayerController>().AddPoints(GameStats.Instance.PointsForNPCS);
                collision.GetComponent<NPC>().KillNPC();
            }
        }

    }
}
