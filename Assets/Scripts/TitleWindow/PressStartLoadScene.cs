using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStartLoadScene : MonoBehaviour {

    private AudioClip submitSound;
    private AudioSource audioSource;

    private int sceneIndex;

    // Use this for initialization
    void Start()
    {
        sceneIndex = 1;
        submitSound = InitiazionGameData.Instance.SubmitSound;
        audioSource = InitiazionGameData.Instance.audioSource;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStart();

    }

    private void CheckStart()
    {
        if (Input.GetButtonDown("Start"))
        {
            sceneIndex = InitiazionGameData.Instance.GetLevelIndex();
            SceneManager.LoadSceneAsync(sceneIndex);   
            audioSource.PlayOneShot(submitSound);
            
        }
    }

    public void SetSceneIndex(int value)
    {
        sceneIndex = value;
    }
}
