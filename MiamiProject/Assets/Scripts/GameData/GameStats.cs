using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour {


    #region GameStats
    [Header("Player")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _speedgab;
    [SerializeField] private float _ControllerThreshhold;

    [Header("GameMode - Last Men Stand")]
    [SerializeField] private int rounds_lms;
    [SerializeField] private float resetDelay_lms;

    [Header("PickUp")]
    [SerializeField] private GameObject _pickUp;
    [SerializeField] private int _maxPickUps;
    [SerializeField] private float _pickUpSpawnDelay;

    [Header("Suckerpunsh")]
    [SerializeField] private GameObject _suckerPunsh;

    [Header("Points")]
    [SerializeField] private int _playerStartScore;
    [SerializeField] private int _pointsForPlayer;
    [SerializeField] private int _pointsForNPCS;
    [SerializeField] private int _pointsForWaldo;
    [SerializeField] private int _pointsForWinning;

    [Header("SpezialWeaponObjects")]
    [SerializeField] private GameObject _explosion;

    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip _song01;
    [SerializeField] private AudioClip _pistolSound;
    [SerializeField] private AudioClip _canonSound;
    [SerializeField] private AudioClip _slowmatchSound;

    [Header("SpezialDebug")]
    [SerializeField]private WeaponCollection.WeaponNames _onlySpawnWeapon;


    #endregion

    #region Setter
    public GameObject SuckerPunsh { get { return _suckerPunsh; } }
    public float WalkSpeed { get { return _walkSpeed; } }
    public float RunSpeed { get { return _runSpeed; } }
    public float Speedgab { get { return _speedgab; } }
    public float ControllerThreshhold { get { return _ControllerThreshhold; } }

    public int Rounds_lms { get { return rounds_lms; } }
    public float RestetDelay_lms { get { return resetDelay_lms; } }

    public GameObject PickUp { get { return _pickUp; } }
    public float PickUpSpawnDelay { get { return _pickUpSpawnDelay; } }
    public int MaxPickUps { get { return _maxPickUps; } }

    public int PointsForPlayer { get { return _pointsForPlayer; } }
    public int PointsForNPCS { get { return _pointsForNPCS; } }
    public int PointsForWaldo { get { return _pointsForWaldo; } }
    public int PointsForWinning { get { return _pointsForWinning; } }
    public int PlayerStartScore { get { return _playerStartScore; } }

    public GameObject Explosion { get { return _explosion; } }

    public AudioSource MusicSource { get { return _musicSource; } }
    public AudioSource SFXSource { get { return _sfxSource; } }
    public AudioClip Song01 { get { return _song01; } }
    public AudioClip PistolSound { get { return _pistolSound; } }
    public AudioClip CanonSound { get { return _canonSound; } }
    public AudioClip SlowmatchSound { get { return _slowmatchSound; } }

    public WeaponCollection.WeaponNames OnlySpawnWeapons { get { return _onlySpawnWeapon; } }

    #endregion

    #region Instance
    public static GameStats Instance;

    void Awake()
    {
        if (Instance != null)
        {
            if (this != Instance)
            {
                Destroy(this);
            }
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        
    }
#endregion
}
