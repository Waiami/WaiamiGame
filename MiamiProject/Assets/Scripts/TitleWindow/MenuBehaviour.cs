using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuBehaviour : MonoBehaviour {
    [SerializeField] private Animator mainMenuAnimator;
    [SerializeField] private Button lastManStanding;
    [SerializeField] private Button newGame;
    [SerializeField] private PlayerReadyButton[] playerButtons;
    [SerializeField] private GameObject FixedCamera;
    private bool pressable;
    private int controllerCount;
    private int playerReadyCount;
    private int[] controllerID = { -1, -1, -1, -1 };
    private enum MenuState { startMenu, gameMode, playerMenu, inGame};
    private MenuState menuState; 
    private string triggerGameMenu = "EnterGameMenu", triggerStartMenu = "EnterStartMenu", triggerPlayerMenu = "EnterPlayerMenu", triggerEnterGame = "EnterGame";
	// Use this for initialization
	void Start () {
        menuState = MenuState.startMenu;
        pressable = true;
        playerReadyCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (menuState != MenuState.inGame && pressable == true)
        {
            CheckButtonPressed();
            CheckControllerButtons();
        }
    }

    void CheckButtonPressed()
    {
        if(menuState != MenuState.startMenu)
        {
            CheckBackButton();
        }
    }

    public void PlayerReady()
    {
        playerReadyCount++;
        if (playerReadyCount > 1 && playerReadyCount >= controllerCount)
        {
            GameController.Instance.InitalizeGameFromMenu(playerReadyCount, controllerID);
            FixedCamera.SetActive(false);
            mainMenuAnimator.SetTrigger(triggerEnterGame);
        }
    }

    private void CheckBackButton()
    {
        if (Input.GetButtonDown("B"))
        {
            if (menuState == MenuState.gameMode)
            {
                pressable = false;
                mainMenuAnimator.SetTrigger(triggerStartMenu);
                StartCoroutine(SelectButton(newGame, MenuState.startMenu));
            }
            if (menuState == MenuState.playerMenu)
            {
                pressable = false;
                mainMenuAnimator.SetTrigger(triggerGameMenu);
                StartCoroutine(SelectButton(lastManStanding, MenuState.gameMode));
                playerReadyCount = 0;
                foreach (PlayerReadyButton b in playerButtons)
                {
                    b.SetPressable(false);
                }
            }
        }
    }

    private void CheckControllerButtons()
    {

        controllerCount = 0;
        string[] name = Input.GetJoystickNames();
        int controllerIDindex = 0;
        for (int i = 1; i <= playerButtons.Length; i++)
        {
            if(name.Length < i)
            {
                return;
            }
            if (string.IsNullOrEmpty(name[i-1]) == false)
            {
                playerButtons[i - 1].GetComponent<Button>().interactable = true;
                controllerCount++;
                if(i <= 4 && i >= 1)
                {
                    controllerID[controllerIDindex] = i- 1;
                }
                else
                {
                    controllerID[controllerIDindex] = -1;
                }
                
            }
            else
            {
                playerButtons[i - 1].GetComponent<Button>().interactable = false;
            }
            controllerIDindex++;
        }
    }

    public void NewGamePressed()
    {
        if (pressable)
        {
            pressable = false;
            mainMenuAnimator.SetTrigger(triggerGameMenu);
            StartCoroutine(SelectButton(lastManStanding, MenuState.gameMode));
        }
        
    }

    public void LastManStandingPressed()
    {
        if (pressable)
        {
            ChangeToPlayerMenu();
        }
    }

    private void ChangeToPlayerMenu()
    {
        pressable = false;
        mainMenuAnimator.SetTrigger(triggerPlayerMenu);
        StartCoroutine(SelectButton(playerButtons[0].GetComponent<Button>(), MenuState.playerMenu));
    }

    public void QuitPressed()
    {
        if(pressable)
            Application.Quit();
    }

    IEnumerator SelectButton(Button buttonToSelect, MenuState stateValue)
    {
        yield return new WaitForSeconds(1f);
        buttonToSelect.Select();
        menuState = stateValue;
        pressable = true;
        if(stateValue == MenuState.playerMenu)
        {
            foreach (PlayerReadyButton b in playerButtons)
            {
                b.SetPressable(true);
            }
        }
    }

    IEnumerator DisableMenu()
    {
        pressable = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    

}
