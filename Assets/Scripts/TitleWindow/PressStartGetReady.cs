using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStartGetReady : MonoBehaviour {
    [SerializeField]
    private GameObject ReadyTextObject;
    [SerializeField]
    private string playerCode = "P1";
    [SerializeField]
    private MapPanelBehaviour mapPanelBehaviour;
    [SerializeField]
    private Animator startanimation;
    [SerializeField]
    private ChangeChar changeChar;
    [SerializeField]
    private GameObject PressStartText;
    [SerializeField]
    private GameObject GetReadyPanel;
    [SerializeField]
    private GameObject OtherGetReady;
    [SerializeField]
    private GameObject OtherReady;


    private AudioClip submitSound;
    private AudioClip cancelSound;
    private AudioSource audioSource;

    private string inputStart = "Start_";
    private string inputB = "B_";


	// Use this for initialization
	void Start () {
        inputStart = inputStart + playerCode;
        inputB = inputB + playerCode;
        submitSound = InitiazionGameData.Instance.SubmitSound;
        cancelSound = InitiazionGameData.Instance.CancelSound;
        audioSource = InitiazionGameData.Instance.audioSource;
	}
	
    void Update()
    {
        CheckButtons();

    }

    private void CheckButtons()
    {
        if (Input.GetButtonDown(inputStart))
        {
            
            ReadyTextObject.SetActive(true);
            mapPanelBehaviour.OnPlayerReady(playerCode);
            this.gameObject.SetActive(false);
            audioSource.PlayOneShot(submitSound);
            changeChar.enabled = false;
        }

        if (Input.GetButtonDown(inputB))
        {
            Debug.Log("Pressed");
            OtherReady.SetActive(false);
            OtherGetReady.SetActive(true);

            GetReadyPanel.SetActive(false);
            PressStartText.SetActive(true);
            startanimation.SetTrigger("Transform");
            audioSource.PlayOneShot(cancelSound);

        }
    }

}
