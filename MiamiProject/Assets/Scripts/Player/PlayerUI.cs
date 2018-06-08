using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private Text pointText;
	

    public void SetPointText(int points)
    {
       // pointText.text = points.ToString();
    }
}
