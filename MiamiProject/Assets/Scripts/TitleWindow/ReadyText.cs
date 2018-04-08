using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyText : MonoBehaviour {

    [SerializeField]
    private string playerCode = "P1";
    private string inputB = "B_";
    [SerializeField]
    private GameObject GetReadyText;
    [SerializeField]
    private MapPanelBehaviour mapPanelBahviour;

    private AudioClip cancelSound;
    private AudioSource audioSource;



    // Use this for initialization
    void Start () {
        inputB = inputB + playerCode;
        cancelSound = InitiazionGameData.Instance.CancelSound;
        audioSource = InitiazionGameData.Instance.audioSource;
    }
	
	// Update is called once per frame
	void Update () {
        CheckButtons();

    }


    private void CheckButtons()
    {

        if (Input.GetButtonDown(inputB))
        {
            mapPanelBahviour.OnPlayerNotReady(playerCode);
            GetReadyText.SetActive(true);
            this.gameObject.SetActive(false);
            audioSource.PlayOneShot(cancelSound);
        }
    }
}
