using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region variables
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    [SerializeField] private GameObject Player3;
    [SerializeField] private GameObject Player4;
    [SerializeField] private GameObject PlayerSpawns;
    [SerializeField] private GameUIView gameUIView;
    [SerializeField] private PickUpSpawnSystem pickUpSpawnSystem;

    private bool player1dead = false, player2dead = false, player3dead = false, player4dead = false;
    private int deadplayers = 0;
    private float resetDelay = 1f;
    private float resetTimer;
    private bool gameFinished = false;
    #endregion

    
    void Start () {
        Initialize();
    }
	
    private void Initialize()
    {
        player1dead = false;
        player2dead = false;
        player3dead = false;
        player4dead = false;
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

    public void PlayerKilled(string playerCode)
    {
        if (gameFinished)
        {
            return;
        }
        if(playerCode == "P1") { player1dead = true; }
        if (playerCode == "P2") { player2dead = true; }
        if (playerCode == "P3") { player3dead = true; }
        if (playerCode == "P4") { player4dead = true; }
        deadplayers++;
        CheckFinishGame();

    }

    private void CheckFinishGame()
    {
        if(deadplayers == 3)
        {
            gameUIView.EnableText();
            gameUIView.SetWinnerText(GetWinningPlayer());
            resetTimer = resetDelay;
            gameFinished = true;
        }
    }

    private string GetWinningPlayer()
    {
        if (!player1dead)
        {
            return "Player 1";
        }else if (!player2dead)
        {
            return "Player 2";
        }
        else if (!player3dead)
        {
            return "Player 3";
        }
        else if (!player4dead)
        {
            return "Player 4";
        }
        else
        {
            return "No One";
        }
    }

    private void ResetGameModeLMS()
    {
        gameUIView.DisableText();
        Initialize();
        Player1.GetComponent<PlayerController>().ResetPlayer();
        Player2.GetComponent<PlayerController>().ResetPlayer();
        Player3.GetComponent<PlayerController>().ResetPlayer();
        Player4.GetComponent<PlayerController>().ResetPlayer();
        pickUpSpawnSystem.SpawnNewPickUps();
    }

    private void SetRandomSpawnPosition()
    {
        int count = PlayerSpawns.transform.childCount;
        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, count).OrderBy(x => rnd.Next()).Take(4).ToList();

        Player1.transform.position = PlayerSpawns.transform.GetChild(randomNumbers[0]).position;
        Player2.transform.position = PlayerSpawns.transform.GetChild(randomNumbers[1]).position;
        Player3.transform.position = PlayerSpawns.transform.GetChild(randomNumbers[2]).position;
        Player4.transform.position = PlayerSpawns.transform.GetChild(randomNumbers[3]).position;
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
