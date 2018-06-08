using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [Header("Animator")]
    [SerializeField] private Animator charAnimator;

    private string[] triggername = new string[] { "MoveDownLeft", "MoveDown","MoveDownRight","MoveRight","MoveUpRight","MoveUp","MoveUpLeft", "MoveLeft"};
    private int count;
    private float multiplicator;
    private int currentState = 0;
    // Use this for initialization
    void Start () {
		if(charAnimator == null)
        {
            charAnimator = gameObject.GetComponent<Animator>();
        }

        count = triggername.Length;
        multiplicator = 1;
        if (count != 0)
        {
            if (count > 6)
            {
                multiplicator = 1 + (1 - 6f / count);
            }
            else if (count < 6)
            {
                multiplicator = (6f / count);
            }
        }
    }

    public void SetAnimationDirection(float x, float y)
    {
        float threshold = 0.1f;
        if (y > threshold && y > (Mathf.Abs(x)))
        {
            charAnimator.SetTrigger("MoveUp");
        }
        else if (y < -threshold && Mathf.Abs(y) > Mathf.Abs(x))
        {
            charAnimator.SetTrigger("MoveDown");
        }
        else if (x > threshold && x > Mathf.Abs(y))
        {
            charAnimator.SetTrigger("MoveLeft");
        }
        else if (x < -threshold && Mathf.Abs(x) > Mathf.Abs(y))
        {
            charAnimator.SetTrigger("MoveRight");
        }
    }

    public void SetBodyAnimationDirection(float x, float y)
    {
        int i = (int)((Mathf.PI + Mathf.Atan2(y + Mathf.PI / count, x + Mathf.PI / count)) * 1.25f);
        //int i = Mathf.FloorToInt((Mathf.Atan2(y, x) * Mathf.Rad2Deg + 180) / 45f);
        if (i != currentState)
        {
            charAnimator.SetTrigger("ChangeState");
            if (i < count && i >=  0)
            {
                charAnimator.SetTrigger(triggername[i]);
                Debug.Log(triggername[i] + " " + i);
                currentState = i;
            }           
            
        }
        
    }

    public void SetAnimationToDead()
    {
        charAnimator.SetTrigger("ChangeState");
        charAnimator.SetTrigger("Dead");
    }

    public void SetBlendFloat(float value)
    {
        charAnimator.SetFloat("Blend", value);
    }

    public void ResetPlayerAnimation()
    {
        charAnimator.SetTrigger("Reset");
    }
}
