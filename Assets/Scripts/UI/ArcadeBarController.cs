using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeBarController : MonoBehaviour
{
    public GameObject foreground;
    public GameObject midground;
    public float fullCatchupTime;
    public bool allowsOvercharge = false;
    public Color overchargeForegroundColor;
    public Color overchargeMidgroundColor;

    private float fullSize;
    private float catchUpPerSecond;
    private Color baseForegroundColor;
    private Color baseMidgroundColor;

    void Start()
    {
        fullSize = foreground.transform.localScale.x;
        catchUpPerSecond = (1f / fullCatchupTime) * fullSize;
        baseForegroundColor = foreground.GetComponent<SpriteRenderer>().color;
        baseMidgroundColor = midground.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (midground != null)
        {
            if (midground.transform.localScale.x > foreground.transform.localScale.x)
            {
                midground.transform.localScale = new Vector2(midground.transform.localScale.x - catchUpPerSecond * Time.deltaTime, midground.transform.localScale.y);
            }
            if (midground.transform.localScale.x < foreground.transform.localScale.x)
            {
                midground.transform.localScale = new Vector2(foreground.transform.localScale.x, midground.transform.localScale.y);
            }
        }
    }

    public void SetDisplayPercentage(float newValue)
    {
        newValue = ((allowsOvercharge ? newValue : Mathf.Clamp(newValue, 0f, 100f)) * fullSize) / 100f;
        foreground.transform.localScale = new Vector2(newValue, foreground.transform.localScale.y);

        if (allowsOvercharge)
        {
            /* ---- Ugly, could refactor */
            if (foreground.transform.localScale.x > fullSize)
            {
                ApplyColor(overchargeForegroundColor, foreground);
            }
            else
            {
                ApplyColor(baseForegroundColor, foreground);
            }

            if (midground.transform.localScale.x > fullSize)
            {
                ApplyColor(overchargeMidgroundColor, midground);
            }
            else
            {
                ApplyColor(baseMidgroundColor, midground);
            }
        }
    }

    public void ApplyColor(Color color, GameObject target)
    {
        if (color != null)
        {
            target.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
