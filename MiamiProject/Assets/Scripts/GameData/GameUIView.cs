using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour {
    [SerializeField] GameObject TextPanel;
    [SerializeField] Text text1;
    [SerializeField] Text text1shadow;

    public void EnableText()
    {
        TextPanel.SetActive(true);
    }

    public void DisableText()
    {
        TextPanel.SetActive(false);
    }

    public void SetWinnerText(string playerName)
    {
        text1.text = playerName + " wins!";
        text1shadow.text = playerName + " wins!";
    }
}
