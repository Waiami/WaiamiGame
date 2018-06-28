using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour{
    private AudioSource menuSound;
    private AudioClip soundButtonChange;
    private AudioClip soundButtonPressed;
    private AudioClip soundWoosh;

    public void OnHover()
    {
        menuSound.PlayOneShot(soundButtonChange);
    }
	// Use this for initialization
	void Start () {
        menuSound = GameStats.Instance.SFXSource;
        soundButtonChange = GameStats.Instance.SoundButtonChanged;
        soundButtonPressed = GameStats.Instance.SoundButtonPressesd;
        soundWoosh = GameStats.Instance.SoundWoosh;
	}

    public void OnButtonPressed()
    {
        menuSound.PlayOneShot(soundButtonPressed);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
