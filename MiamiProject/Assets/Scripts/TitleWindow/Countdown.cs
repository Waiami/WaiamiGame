using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
    [SerializeField] private Text timerText;
    [SerializeField] private string endText = "Go!";
    public void Start()
    {
        SetCountdown(3);
    }

    private void OnEnable()
    {
        SetCountdown(3);
    }
    public void SetCountdown(int time)
    {
        StartCoroutine(StartCountdown(time));
    }

    private IEnumerator StartCountdown(int time)
    {
        timerText.text = time.ToString();
        while(time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            timerText.text = time.ToString();
        }
        timerText.text = endText;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
