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
