using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyButton : MonoBehaviour {

    [SerializeField] private string PlayerCode = "1";
    [SerializeField] private MenuBehaviour menuBehaviour;
    private bool playerReady;
    private bool pressable;
    public void SetPressable(bool value)
    {
        pressable = value;
        this.GetComponentInChildren<Text>().text = "Player " + PlayerCode;
        playerReady = false;
    }

	// Use this for initialization
	void Start () {
        playerReady = false;
        pressable = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (pressable)
        {
            CheckPlayerReady();
        }
    }

    private void CheckPlayerReady()
    {
        if (Input.GetButtonDown("A_P"+PlayerCode))
        {
            if (!playerReady)
            {
                SetPlayerReady();
            }
        }
    }

    private void SetPlayerReady()
    {
        playerReady = true;
        this.GetComponentInChildren<Text>().text = "Player " + PlayerCode + " ready";
        menuBehaviour.PlayerReady();
    }
}
