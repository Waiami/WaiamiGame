using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSprite : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField]
    private string playerCode = "P1";

	// Use this for initialization
	void Start () {
        SetSprite();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetSprite()
    {
        if (InitiazionGameData.Instance != null)
        {
            if (playerCode == "P1")
            {

                playerSpriteRenderer.sprite = InitiazionGameData.Instance.GetPlayerSprite(1);
            }
            else
            {
                playerSpriteRenderer.sprite = InitiazionGameData.Instance.GetPlayerSprite(2);
            }

        }

    }

}
