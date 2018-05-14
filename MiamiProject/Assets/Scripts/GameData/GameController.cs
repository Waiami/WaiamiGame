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

    private int deadplayers = 0;
    private float resetDelay = 1f;
    private float resetTimer;
    private bool gameFinished = false;
    private AudioSource musikSource;
    #endregion

    
    void Start () {
        Initialize();
        PlayChoosenSong();
    }
	
    private void PlayChoosenSong()
    {
        musikSource.clip = GameStats.Instance.Song01;
        musikSource.Play();
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
        for(int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = PlayerSpawns.transform.GetChild(randomNumbers[i]).position;
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
