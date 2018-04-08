using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiazionGameData : MonoBehaviour {

    [Header("Sounds")]
    [SerializeField]
    private AudioClip _submitSound;
    [SerializeField]
    private AudioClip _cancelSound;
    [SerializeField]
    private AudioSource _audioSource;

    private Sprite p1Sprite;
    private Sprite p2Sprite;
    private int levelIndex;

    public AudioClip SubmitSound { get { return _submitSound; } }
    public AudioClip CancelSound { get { return _cancelSound; } }
    public AudioSource audioSource { get { return _audioSource; } }

    public static InitiazionGameData Instance;


    private void Awake()
    {
        if(Instance != null)
        {
            if(this != Instance)
            {
                Destroy(this);
            }
        }
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetLevelIndex(int i)
    {
        levelIndex = i + 1;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public void SetPlayerSprite(Sprite pSprite, int i)
    {
        if(i == 1)
        {
            p1Sprite = pSprite;
        }else if(i == 2)
        {
            p2Sprite = pSprite;
        }
    }

    public Sprite GetPlayerSprite(int i)
    {
        Sprite returnSprite;
        if(i == 1)
        {
            returnSprite = p1Sprite;
        }
        else
        {
            returnSprite = p2Sprite;
        }
        return returnSprite;

    }
}
