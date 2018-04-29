using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel : MonoBehaviour {

#region variables
    [SerializeField] private string playerCode = "P1";
    [SerializeField] private float walkspeed = 5;
    [SerializeField] private float runspeed = 7;
    [SerializeField] private float speedgab = 0.7f;
    [SerializeField] private float controllerthreshhold = 0.3f;

    [SerializeField] private PlayerController playerController;
    private Queue<Weapon> playerWeaponList;
    private Weapon equippedWeapon;
    private bool isDead;
    private float throwAwayTime = 0;

    public bool IsDead { get { return isDead; } }
    public float WalkSpeed { get { return walkspeed; } }
    public float RunSpeed { get { return runspeed; } }
    public float Speedgab { get { return speedgab; } }
    public float ControllerThreshhold { get { return controllerthreshhold; } }
    public string PlayerCode { get { return playerCode; } }
#endregion

    // Use this for initialization
    void Start () {
        isDead = false; 
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
        controllerthreshhold = GameStats.Instance.ControllerThreshhold;
        if(playerController == null)
        {
            playerController = gameObject.GetComponent<PlayerController>();
        }
    }
    
    public WeaponCollection.WeaponNames GetEquippedWeapon()
    {
        if(equippedWeapon == null)
        {
            return WeaponCollection.WeaponNames.empty;
        }
        return equippedWeapon.WeaponName;
    }

    public void KillPlayer()
    {
        if (isDead)
        {
            return;
        }
        playerController.SetPlayerToDead();
        GameController.Instance.PlayerKilled(playerCode);
        isDead = true;
    }

    public void AddNewWeapon(Weapon weapon)
    {
        playerWeaponList.Enqueue(weapon);
        equippedWeapon = weapon;
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
            equippedWeapon = playerWeaponList.Dequeue();
            if(playerWeaponList.Count < 1)
            {
                equippedWeapon = null;
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

    public void ResetPlayer()
    {
        isDead = false;
        playerWeaponList.Clear();
        equippedWeapon = null;
    }
}
