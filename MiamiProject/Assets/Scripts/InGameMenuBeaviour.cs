using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuBeaviour : MonoBehaviour {

    [SerializeField]
    private GameObject inGamePanel;
    private bool toggle;
    // Use this for initialization
    void Start()
    {

        toggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (!toggle)
            {
                inGamePanel.SetActive(true);
                toggle = true;
            }
            else
            {
                inGamePanel.SetActive(false);
                toggle = false;
            }
        }
    }
}
