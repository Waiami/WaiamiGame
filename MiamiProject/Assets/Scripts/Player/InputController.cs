using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    #region variables
    private PlayerController playerController;
    private string inputHorizontal = "Horizontal_";
    private string inputVertical = "Vertical_";
    private string inputRotateHorizontal = "RotateHorizontal_";
    private string inputRotateVertical = "RotateVertical_";
    private string aButton = "A_";
    private string fireButton = "Fire_";
#endregion

    void Start()
    {
        Initialize();
    }

    #region initialize
    void Initialize()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        string playerCode = playerController.PlayerModelScript.PlayerCode;
        inputHorizontal = inputHorizontal + playerCode;
        inputVertical = inputVertical + playerCode;
        inputRotateHorizontal = inputRotateHorizontal + playerCode;
        inputRotateVertical = inputRotateVertical + playerCode;
        aButton = aButton + playerCode;
        fireButton = fireButton + playerCode;
    }
    #endregion

    private void FixedUpdate()
    {
        if (!playerController.PlayerModelScript.IsDead)
        {
            MovePlayer();
            RotatePlayer();
        }

    }

    private void Update()
    {
        if (!playerController.PlayerModelScript.IsDead)
        {
            CheckIfFireButtonPressed();
            CheckIfInteraktionButtonPressed();
        }

    }

    private void MovePlayer()
    {
        float x = Input.GetAxis(inputHorizontal);
        float y = Input.GetAxis(inputVertical);     
        if (Mathf.Abs(x) < playerController.PlayerModelScript.ControllerThreshhold) { x = 0; }
        if (Mathf.Abs(y) < playerController.PlayerModelScript.ControllerThreshhold) { y = 0; }
        playerController.MovePlayer(x, y);
    }

    private void RotatePlayer()
    {
        float x = Input.GetAxis(inputRotateHorizontal);
        float y = Input.GetAxis(inputRotateVertical);
        playerController.RotatePlayer(x, y);
    }

    private void CheckIfInteraktionButtonPressed()
    {
        if (Input.GetButton(aButton))
        {
            playerController.ExecuteInteractionButton();
        }
    }

    private void CheckIfFireButtonPressed()
    {
        var x = Input.GetAxis(fireButton);
        if (x > 0.7f)
        {
            playerController.CheckFireWeapon();
        }
    }
}
