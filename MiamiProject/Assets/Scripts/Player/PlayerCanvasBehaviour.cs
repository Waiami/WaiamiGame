using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasBehaviour : MonoBehaviour {
    [SerializeField] private GameObject totalPoints;
    [SerializeField] private GameObject FloatingText;
    [SerializeField] private Transform popUpParent;

    public void PopUpScore(string newPoints, string totalScore)
    {

        GameObject go = Instantiate(FloatingText, popUpParent);
        go.GetComponent<Text>().text = newPoints;
        totalPoints.GetComponent<Text>().text = totalScore;
        totalPoints.GetComponent<Animator>().SetTrigger("AddScore");
    }
}
