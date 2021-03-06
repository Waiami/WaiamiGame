﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region variables
    [SerializeField] private PlayerController[] players;
    [SerializeField] private GameObject PlayerSpawns;
    [SerializeField] private GameUIView gameUIView;
    [SerializeField] private PickUpSpawnSystem pickUpSpawnSystem;
    [SerializeField][Range (1,4)] private int PlayerCount;
    [SerializeField] private GameObject[] PlayerPrefabs;
    [SerializeField] private GameObject[] CameraPrefabs;
    [SerializeField] private GameObject[] TwoPlayerCameraPrefabs;
    [SerializeField] private GameObject windowBar;
    [SerializeField] private GameObject CountDownPanel;

    private int deadplayers = 0;
    private int gameRound = 1;
    private int[] controllerID = {0, 1, 2, 3 };
    private float resetDelay = 1f;
    private float resetTimer;
    private bool gameFinished = false;
    private AudioSource musikSource;

    #endregion

    
    void Start () {
        //CreatePlayers();
        //Initialize();
        
        //PlayChoosenSong();
    }
	
    private void PlayChoosenSong()
    {
        ulong sample = (ulong)Random.Range(0, 44100);
        musikSource.clip = GameStats.Instance.Song01;
        musikSource.Play(sample);
    }

    private void CreatePlayers()
    {
        int cameraIndex = 0;
        for (int i = 0; controllerID.Length > i; i++)
        {
            if (controllerID[i] != -1)
            {
                int index = controllerID[i];
                players[index] = Instantiate(PlayerPrefabs[0]).GetComponent<PlayerController>();
                players[index].SetPlayer("P" + (index + 1), "Player " + (index + 1));
                GameObject cam;
                if (PlayerCount == 2)
                {
                    cam = Instantiate(TwoPlayerCameraPrefabs[cameraIndex]);
                    cameraIndex++;
                }
                else
                {
                    cam = Instantiate(CameraPrefabs[index]);
                }
                cam.GetComponent<CameraFollow>().SetPlayerPoint(players[index].GetCameraPoint());
                players[index].SetCameraFollow(cam.GetComponent<CameraFollow>());
                
            }
            else
            {
                if(PlayerCount == 3)
                {
                    for(int j = 0; j < controllerID.Length; j++)
                    {
                        if(j != controllerID[0] && j != controllerID[1] && j != controllerID[2] && j != controllerID[3])
                        {
                            Instantiate(CameraPrefabs[j]);
                            break;
                        }
                    }  
                }
            }
            
        }

    }

    private void Initialize()
    {
        musikSource = GameStats.Instance.MusicSource;
        deadplayers = 0;
        resetDelay = GameStats.Instance.RestetDelay_lms;
        gameFinished = false;
        pickUpSpawnSystem.SpawnNewPickUps();
        SetRandomSpawnPosition();
        if(gameUIView == null)
        {
            gameUIView = gameObject.GetComponent<GameUIView>();
        }
    }
	
	void Update () {
        if (gameFinished)
        {
            if(resetTimer < 0)
            {
                gameRound++;
                if(gameRound == GameStats.Instance.Rounds_lms)
                {
                    //Show Endstats
                }
                else
                {
                    ResetGameModeLMS();
                }
                
               
            }
            else
            {
                resetTimer -= Time.deltaTime;
            }
        }
	}

    public void InitalizeGameFromMenu(int playerCount, int[] controllerID)
    {
        PlayerCount = playerCount;
        this.controllerID = controllerID;
        CreatePlayers();
        Initialize();
        PlayChoosenSong();
        if(playerCount == 2)
        {
            windowBar.SetActive(false);
        }
        else
        {
            windowBar.SetActive(true);
        }
        musikSource.pitch = 1;
        musikSource.panStereo = 0;
        musikSource.volume = 1;
        SetPlayerMoveable(false);
        CountDownPanel.SetActive(true);
        StartCoroutine(WaitToSetPlayerMovement(4, true));

    }

    public void PlayerKilled()
    {
        if (gameFinished)
        {
            return;
        }
        deadplayers++;
        CheckFinishGame();
    }
    
    private void SetPlayerMoveable(bool value)
    {
        foreach(PlayerController p in players)
        {
            if(p != null)
            {
                p.CantMove = value;
            }
        }
    }

    private IEnumerator WaitToSetPlayerMovement(int time, bool value)
    {
        yield return new WaitForSeconds(time);
        SetPlayerMoveable(value);
    }

    private void CheckFinishGame()
    {
        if(deadplayers == PlayerCount - 1)
        {
            PlayerController winningPlayer = GetWinningPlayer();
            gameUIView.EnableText();
            gameUIView.SetWinnerText(winningPlayer.GetComponent<PlayerDataModel>().PlayerName);
            winningPlayer.AddPoints(GameStats.Instance.PointsForWinning);
            resetTimer = resetDelay;
            gameFinished = true;
        }
    }

    private PlayerController GetWinningPlayer()
    {
        foreach(PlayerController pc in players)
        {
            if(pc != null)
            {
                if (!pc.GetComponent<PlayerDataModel>().IsDead)
                {
                    return pc;
                }
            }
        }
        return null;
    }

    private void ResetGameModeLMS()
    {
        gameUIView.DisableText();
        Initialize();
        foreach (PlayerController pc in players)
        {
            if(pc != null)
            {
                pc.ResetPlayer();
            }
        }
        pickUpSpawnSystem.SpawnNewPickUps();
    }

    private void SetRandomSpawnPosition()
    {
        int count = PlayerSpawns.transform.childCount;
        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, count).OrderBy(x => rnd.Next()).Take(4).ToList();
        for(int i = 0; i < controllerID.Length; i++)
        {
            if(controllerID[i]!= -1)
            {
                players[i].transform.position = PlayerSpawns.transform.GetChild(randomNumbers[i]).position;
            }         
        }
    }


    public PlayerController GetPlayerController(int i)
    {
        return players[i];
    }


    #region Instance
    public static GameController Instance;

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
