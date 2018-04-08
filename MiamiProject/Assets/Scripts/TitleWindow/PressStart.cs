using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour {

    [SerializeField]
    private Animator startanimation;
    [SerializeField]
    private GameObject getReadyPanel;

    private AudioClip submitSound;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        submitSound = InitiazionGameData.Instance.SubmitSound;
        audioSource = InitiazionGameData.Instance.audioSource;
    }
	
	// Update is called once per frame
	void Update () {
        CheckStart();

    }

    private void CheckStart()
    {
        if (Input.GetButtonDown("Start"))
        {
            startanimation.SetTrigger("Transform");
            getReadyPanel.SetActive(true);
            this.gameObject.SetActive(false);
            audioSource.PlayOneShot(submitSound);
        }
    }
}
