using System.Collections;
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

    private int deadplayers = 0;
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
        musikSource.clip = GameStats.Instance.Song01;
        musikSource.Play();
    }

    private void CreatePlayers()
    {
        int cameraIndex = 0;
        for (int i = 0; controllerID.Length > i; i++)
        {
            if (controllerID[i] != -1)
            {
                int index = controllerID[i];
                players[index] = Instantiate(PlayerPrefabs[index]).GetComponent<PlayerController>();
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
                ResetGameModeLMS();
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

    private void CheckFinishGame()
    {
        if(deadplayers == 3)
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
            if (!pc.GetComponent<PlayerDataModel>().IsDead)
            {
                return pc;
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
            pc.ResetPlayer();
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
