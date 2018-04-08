using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMap : MonoBehaviour {
    [SerializeField]
    private string[] mapNames;
    [SerializeField]
    private int selectedMap = 0;
    [SerializeField]
    private Text mapText;
    [SerializeField]
    private float inputDelay = 0.3f;
    private float timer = 0;
    [SerializeField]
    private int lastMaps = 1;

    private string inputHorizontal = "Horizontal";

    private AudioClip submitSound;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        submitSound = InitiazionGameData.Instance.SubmitSound;
        audioSource = InitiazionGameData.Instance.audioSource;
        lastMaps = mapNames.Length - 1;
        changeMapText();
    }

    // Update is called once per frame
    void Update()
    {
        CheckButtons();

    }


    private void CheckButtons()
    {
        if (timer <= 0)
        {
            if (Input.GetAxis(inputHorizontal) < -0.5)
            {
                selectedMap--;
                if (selectedMap < 0)
                {
                    selectedMap = lastMaps;
                }
                audioSource.PlayOneShot(submitSound);
                timer = inputDelay;
                changeMapText();
            }

            if (Input.GetAxis(inputHorizontal) > 0.5)
            {
                selectedMap++;
                if (selectedMap > lastMaps)
                {
                    selectedMap = 0;
                }
                audioSource.PlayOneShot(submitSound);
                timer = inputDelay;
                changeMapText();

            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }

    private void changeMapText()
    {
        mapText.text = mapNames[selectedMap];
        InitiazionGameData.Instance.SetLevelIndex(selectedMap);
    }


}
