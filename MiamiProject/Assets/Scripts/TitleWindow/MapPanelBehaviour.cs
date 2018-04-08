using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanelBehaviour : MonoBehaviour {

    private bool p1ready = false;
    private bool p2ready = false;
    private bool mapChooseReady;

    [SerializeField]
    private ChangeChar changeCharP1;
    [SerializeField]
    private ChangeChar changeCharP2;
    [SerializeField]
    private GameObject chooseMapObject;

    public void OnPlayerReady(string playerCode)
    {
        if(playerCode == "P1")
        {
            p1ready = true;
        }
        if(playerCode == "P2")
        {
            p2ready = true;
        }

        if (p1ready && p2ready)
        {
            chooseMapObject.SetActive(true);
        }
    }

    public void OnPlayerNotReady(string playerCode)
    {
        if (playerCode == "P1")
        {
            p1ready = false;
            changeCharP1.enabled = true;
        }
        if (playerCode == "P2")
        {
            p2ready = false;
            changeCharP2.enabled = true;
        }

        chooseMapObject.SetActive(false);
    }


}
