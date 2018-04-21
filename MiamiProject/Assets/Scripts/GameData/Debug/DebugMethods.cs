using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMethods : MonoBehaviour {

    [SerializeField]
    GameObject[] perosnalCameras;
    [SerializeField]
    GameObject globalCamera;
    [SerializeField] DebugSpawner debugSpawner;
    [SerializeField] GameObject DebugInfoPanel;


	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (globalCamera.activeInHierarchy)
            {
                ActivatePersonalCamera();
            }
            else
            {
                ActivateGlobalCamera();
            }
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            debugSpawner.SpawnNewPickUps();
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (DebugInfoPanel.activeInHierarchy)
            {
                DebugInfoPanel.SetActive(false);
            }
            else
            {
                DebugInfoPanel.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    private void ActivatePersonalCamera()
    {
        globalCamera.SetActive(false);
        foreach(GameObject camera in perosnalCameras)
        {
            camera.SetActive(true);
        }
    }

    private void ActivateGlobalCamera()
    {
        foreach (GameObject camera in perosnalCameras)
        {
            camera.SetActive(false);
        }
        globalCamera.SetActive(true);
        
    }
}
