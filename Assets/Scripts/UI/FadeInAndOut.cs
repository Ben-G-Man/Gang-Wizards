using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInAndOut : MonoBehaviour
{
    public enum autoFadeOption
    {
        NoAutoFade,
        FadeIn,
        FadeOut
    }

    public autoFadeOption autoFade;
    public float autoFadeDuration;

    private string nextScene;
    private float target;
    private float diff;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (autoFade == autoFadeOption.FadeIn) FadeIn(autoFadeDuration);
        else if (autoFade == autoFadeOption.FadeOut) FadeOut(autoFadeDuration);
        else
        {
            target = sr.color.a;
            diff = 0f;
        }
    }

    public void FadeIn(float duration)
    {
        FadeTo(1f, duration);
    }

    public void FadeOut(float duration)
    {
        FadeTo(0f, duration);
    }

    public void FadeTo(float target, float duration)
    {
        if (target == sr.color.a) return;
        this.target = target;
        diff = GetDiff(sr.color.a, target, duration);
    }

    private float GetDiff(float current, float target, float duration)
    {
        return (target - current) / duration;
    }

    void Update()
    {
        float next = sr.color.a + diff * Time.deltaTime;
        next = (diff > 0) ? Mathf.Clamp(next, 0f, target) : Mathf.Clamp(next, target, 255f);
        sr.color = new Color(
            sr.color.r,
            sr.color.g,
            sr.color.b,
            next
        );
        
        if (next.Equals(target) && nextScene != null) 
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void SetNextScene(string nextScene)
    {
        this.nextScene = nextScene;
    }
}
