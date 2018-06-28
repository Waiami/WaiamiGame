using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel : MonoBehaviour {

#region variables
    [SerializeField] private string playerCode = "P1";
    [SerializeField] private string playerName;
    [SerializeField] private float walkspeed = 5;
    [SerializeField] private float runspeed = 7;
    [SerializeField] private float speedgab = 0.7f;
    [SerializeField] private float controllerthreshhold = 0.3f;

    private Queue<Weapon> playerWeaponList;
    private Weapon equippedWeapon;
    private bool dead;
    private float throwAwayTime = 0;

    private int playerScore;
    private int pointsWorth;

    public bool IsDead { get { return dead; } }
    public float WalkSpeed { get { return walkspeed; } }
    public float RunSpeed { get { return runspeed; } }
    public float Speedgab { get { return speedgab; } }
    public float ControllerThreshhold { get { return controllerthreshhold; } }
    public string PlayerCode { get { return playerCode; } }
    public string PlayerName { get { return playerName; } }
    public int PlayerScore { get { return playerScore; } }
    public int PointsWorth { get { return pointsWorth; } }
    #endregion

    // Use this for initialization
    void Start () {
        dead = false;
        playerWeaponList = new Queue<Weapon>();
        if(playerWeaponList.Count > 0)
        {
            Weapon w = playerWeaponList.Peek();
        }
        Inisialize();
    }

    void Inisialize()
    {
        walkspeed = GameStats.Instance.WalkSpeed;
        runspeed = GameStats.Instance.RunSpeed;
        speedgab = GameStats.Instance.Speedgab;
        pointsWorth = GameStats.Instance.PointsForPlayer;
        controllerthreshhold = GameStats.Instance.ControllerThreshhold;
    }
    
    public Weapon GetEquippedWeapon()
    {
        if(equippedWeapon == null)
        {
            return null;
        }
        return equippedWeapon;
    }

    public void KillPlayer()
    {
        dead = true;
    }

    public void AddNewWeapon(Weapon weapon)
    {
        playerWeaponList.Enqueue(weapon);
        if(playerWeaponList.Count == 1)
        {
            equippedWeapon = weapon;
        }
        
        DebugLogWeaponList();
    }

    public int GetWeaponListCount()
    {
        return playerWeaponList.Count;
    }

    public void DeleteWeaponFifo()
    {
        equippedWeapon = null;
        if(playerWeaponList.Count > 0)
        {
            playerWeaponList.Dequeue();
            if (playerWeaponList.Count < 1)
            {
                equippedWeapon = null;
            }
            else
            {
                equippedWeapon = playerWeaponList.Peek();
            }
            DebugLogWeaponList();
        }
    }

    public float GetCooldownTime()
    {
        float returnValue = 0.3f;
        if(playerWeaponList.Count == 0)
        {
            return 0;
        }
        Weapon weapon = playerWeaponList.Peek();
        returnValue = weapon.EqiupTime + throwAwayTime;
        return returnValue;
    }

    public void SetTrowAwayTime()
    {
        if(playerWeaponList.Count != 0)
        {
            Weapon weapon = playerWeaponList.Peek();
            throwAwayTime = weapon.ThrowAwayTime;
        }
    }

    private void DebugLogWeaponList()
    {
        if(playerWeaponList.Count > 0)
        {
            string returnstring = "";
            foreach (Weapon s in playerWeaponList)
            {
                returnstring += s.name + ", ";
            }
            Debug.Log(returnstring);
        }
        else
        {
            Debug.Log("No Weapons left");
        }
        
    }

    public void AddPoints(int points)
    {
        playerScore += points;
        if (playerScore < 0) playerScore = 0;

    }

    public void ResetPlayer()
    {
        dead = false;
        if(playerWeaponList != null)
        {
            playerWeaponList.Clear();
        }
        equippedWeapon = null;
    }
}
