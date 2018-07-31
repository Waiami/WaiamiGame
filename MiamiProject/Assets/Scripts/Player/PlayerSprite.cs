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
    [SerializeField] private Sprite[] CharSprites;
    [SerializeField] private SpriteRenderer headSpriteRenderer;
    [SerializeField] private Sprite[] HeadSprites;
    private int count;
    private int currentState;
    private float multiplicator;

    private void Start()
    {       
        count = CharSprites.Length;
        multiplicator = 1;
        if(count != 0)
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

    public void SetHeadSpriteToRotation(float x, float y)
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

    public void SetHeadDirectionToBody(float x, float y)
    {
        int i = (int)((Mathf.PI + Mathf.Atan2(y + Mathf.PI / 8, x + Mathf.PI / 8)) * 1.25f);
        //int i = Mathf.FloorToInt((Mathf.Atan2(y, x) * Mathf.Rad2Deg + 180) / 45f);
        if (i != currentState)
        {
            if (i < count && i >= 0)
            {
                currentState = i;
                switch (i)
                {
                    case 0:
                        headSpriteRenderer.sprite = HeadSprites[3];
                        headSpriteRenderer.flipX = true;
                        break;
                    case 1:
                        headSpriteRenderer.sprite = HeadSprites[1];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 2:
                        headSpriteRenderer.sprite = HeadSprites[3];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 3:
                        headSpriteRenderer.sprite = HeadSprites[2];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 4:
                        headSpriteRenderer.sprite = HeadSprites[4];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 5:
                        headSpriteRenderer.sprite = HeadSprites[0];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 6:
                        headSpriteRenderer.sprite = HeadSprites[4];
                        headSpriteRenderer.flipX = true;
                        break;
                    case 7:
                        headSpriteRenderer.sprite = HeadSprites[2];
                        headSpriteRenderer.flipX = true;
                        break;

                }
            }
            if(x == 0 && y == 0)
            {
                headSpriteRenderer.sprite = HeadSprites[1];
            }

        }

    }

    public void SetHeadDirection(float x, float y)
    {
        int i = (int)((Mathf.PI + Mathf.Atan2(y + Mathf.PI / 8, x + Mathf.PI / 8)) * 1.25f);
        //int i = Mathf.FloorToInt((Mathf.Atan2(y, x) * Mathf.Rad2Deg + 180) / 45f);
        if (i != currentState)
        {
            //Debug.Log(i);
            if (i < count && i >= 0)
            {
                currentState = i;
                switch (i)
                {
                    case 0:
                        headSpriteRenderer.sprite = HeadSprites[3];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 1:
                        headSpriteRenderer.sprite = HeadSprites[1];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 2:
                        headSpriteRenderer.sprite = HeadSprites[3];
                        headSpriteRenderer.flipX = true;
                        break;
                    case 3:
                        headSpriteRenderer.sprite = HeadSprites[2];
                        headSpriteRenderer.flipX = true;
                        break;
                    case 4:
                        headSpriteRenderer.sprite = HeadSprites[4];
                        headSpriteRenderer.flipX = true;
                        break;
                    case 5:
                        headSpriteRenderer.sprite = HeadSprites[0];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 6:
                        headSpriteRenderer.sprite = HeadSprites[4];
                        headSpriteRenderer.flipX = false;
                        break;
                    case 7:
                        headSpriteRenderer.sprite = HeadSprites[2];
                        headSpriteRenderer.flipX = false;
                        break;

                }
            }

        }

    }

    public void SetBodySpriteToRotation(float x, float y)
    {
        int i = (int)((Mathf.PI + Mathf.Atan2(y + Mathf.PI / count, x + Mathf.PI / count)) * multiplicator);
        playerSpriteRenderer.sprite = CharSprites[i];
    }

    public void SetBodySpriteToDead()
    {
        playerSpriteRenderer.sprite = CharDead;
    }

    public void ResetPlayerSprite()
    {
        playerSpriteRenderer.sprite = CharUp;
    }

}
