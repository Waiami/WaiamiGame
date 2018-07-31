using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    [SerializeField] private Animator animator;
    private Text pointText;

    // Use this for initialization
    void Start()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject,clipInfo[0].clip.length);
        pointText = animator.GetComponent<Text>();
    }


    public void SetText(string text)
    {
        pointText.text = text;
    }
}
