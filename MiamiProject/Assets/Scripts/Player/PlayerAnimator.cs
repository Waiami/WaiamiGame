using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [Header("Animator")]
    [SerializeField] private Animator charAnimator;

    // Use this for initialization
    void Start () {
		if(charAnimator == null)
        {
            charAnimator = gameObject.GetComponent<Animator>();
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

    public void SetAnimationToDead()
    {
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
