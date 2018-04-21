using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour {

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Sprite CharUp;
    [SerializeField] private Sprite CharDown;
    [SerializeField] private Sprite CharLeft;
    [SerializeField] private Sprite CharRight;
    [SerializeField] private Sprite CharDead;


    public void SetBodySpriteToRotation(float x, float y)
    {
        float thresholded = 0.2f;
        if (x != 0 && y != 0)
        {
            if (y > thresholded && y > (Mathf.Abs(x)))
            {
                playerSpriteRenderer.sprite = CharUp;
            }
            else if (y < -thresholded && Mathf.Abs(y) > Mathf.Abs(x))
            {
                playerSpriteRenderer.sprite = CharDown;
            }
            else if (x > thresholded && x > Mathf.Abs(y))
            {
                playerSpriteRenderer.sprite = CharLeft;
            }
            else if (x < -thresholded && Mathf.Abs(x) > Mathf.Abs(y))
            {
                playerSpriteRenderer.sprite = CharRight;
            }
        }
    }

    public void SetBodySpriteToDead()
    {
        playerSpriteRenderer.sprite = CharDead;
    }

}
