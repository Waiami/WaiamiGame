using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static GameData Instance;

    [SerializeField]
    private GameObject _suckerPunsh;
    [SerializeField]
    private GameObject _fivemmBullet;


    public GameObject SuckerPunsh { get { return _suckerPunsh; } }
    public GameObject FivemmBullet { get {return _fivemmBullet; }}
    

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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
