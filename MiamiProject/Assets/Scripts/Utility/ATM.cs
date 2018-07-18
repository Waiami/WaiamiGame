using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATM : MonoBehaviour {
    [SerializeField] private Text pointtext;
    private Collider2D currentCollision;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDataModel pdm = collision.GetComponent<PlayerDataModel>();
        if(pdm != null && currentCollision == null)
        {
            pointtext.gameObject.SetActive(true);
            pointtext.text = pdm.PlayerScore.ToString();
            currentCollision = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentCollision = collision)
        {
            currentCollision = null;
            pointtext.gameObject.SetActive(false);
        }
    }
}
