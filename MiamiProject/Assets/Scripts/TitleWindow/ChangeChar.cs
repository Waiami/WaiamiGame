using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeChar : MonoBehaviour {
    [SerializeField]
    private Sprite[] charSprites;
    [SerializeField]
    private string playerCode = "P1";
    [SerializeField]
    private int selectedSprite = 1;
    [SerializeField]
    private Image charImage;
    [SerializeField]
    private float inputDelay = 0.5f;
    private float timer = 0;
    private int spriteArrayLast = 0;

    private string inputHorizontal = "Horizontal_";

    private AudioClip submitSound;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        inputHorizontal = inputHorizontal + playerCode;
        submitSound = InitiazionGameData.Instance.SubmitSound;
        audioSource = InitiazionGameData.Instance.audioSource;
        spriteArrayLast = charSprites.Length - 1;
        changeSprite();
    }
	
	// Update is called once per frame
	void Update () {
        CheckButtons();

    }


    private void CheckButtons()
    {
        if(timer <= 0)
        {
            if (Input.GetAxis(inputHorizontal) < -0.19)
            {
                selectedSprite--;
                if (selectedSprite < 0)
                {
                    selectedSprite = spriteArrayLast;
                }
                audioSource.PlayOneShot(submitSound);
                timer = inputDelay;
                changeSprite();
            }

            if (Input.GetAxis(inputHorizontal) > 0.19)
            {
                selectedSprite++;
                if (selectedSprite > spriteArrayLast)
                {
                    selectedSprite = 0;
                }
                audioSource.PlayOneShot(submitSound);
                timer = inputDelay;
                changeSprite();
                
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
    }

    private void changeSprite()
    {
        charImage.sprite = charSprites[selectedSprite];
        if(playerCode== "P1")
        {
            InitiazionGameData.Instance.SetPlayerSprite(charSprites[selectedSprite], 1);
        }
        else
        {
            InitiazionGameData.Instance.SetPlayerSprite(charSprites[selectedSprite], 2);
        }
    }

    public Sprite GetSelectedChar()
    {
        return charSprites[selectedSprite];
    }
}
