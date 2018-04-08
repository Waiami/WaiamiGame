using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMethods : MonoBehaviour {

    [SerializeField]
    GameObject[] perosnalCameras;
    [SerializeField]
    GameObject globalCamera;

    private bool cameraToggle = false;
	// Use this for initialization
	void Start () {
		
	}

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!cameraToggle)
            {
                cameraToggle = true;
                ActivatePersonalCamera();
            }
            else
            {
                cameraToggle = false;
                ActivateGlobalCamera();
            }
            
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
