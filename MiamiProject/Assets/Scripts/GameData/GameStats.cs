﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour {


    #region GameStats
    [Header("Player")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _speedgab;
    [SerializeField] private float _threshhold;
    [Space(20)]
    [Header("Weapons")]

    [Header("GeneralWeapons")]
    [SerializeField] private List<string>_weaponList = new List<string> {"pistol"};
    [SerializeField] private float _attackDelay = 0.3f;


    [Header("Suckerpunsh")]
    [SerializeField] private GameObject _suckerPunsh;

    [Header("Pistol")]
    [SerializeField] private GameObject _fivemmBullet;


    #endregion


    #region Setter
    public GameObject SuckerPunsh { get { return _suckerPunsh; } }
    public GameObject FivemmBullet { get {return _fivemmBullet; }}
    public float WalkSpeed { get { return _walkSpeed; } }
    public float RunSpeed { get { return _runSpeed; } }
    public float Speedgab { get { return _speedgab; } }
    public float Threshhold { get { return _threshhold; } }
    public float AttackDelay { get { return _attackDelay; } }
    public List<string> WeaponList { get { return _weaponList; } }

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